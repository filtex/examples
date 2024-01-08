const { MongoClient } = require('mongodb');
const {data} = require("./data");

class MongoStorage {
    constructor() {
        this._mongoURI = 'mongodb://127.0.0.1:27017';
        this._databaseName = 'filtex';
        this._collectionName = 'projects';
    }

    async Init() {
        const client = new MongoClient(this._mongoURI);

        try {
            await client.connect();

            const database = client.db(this._databaseName);

            await database.dropCollection(this._collectionName);

            const collection = database.collection(this._collectionName);

            const docs = data.map(x => ({
                name: x.name,
                version: x.version,
                tags: x.tags,
                status: x.status,
                createdAt: x.createdAt,
            }));

            await collection.insertMany(docs);
        } finally {
            await client.close();
        }
    }

    async Query(query) {
        const client = new MongoClient(this._mongoURI);

        try {
            await client.connect();

            const database = client.db(this._databaseName);

            const collection = database.collection(this._collectionName);

            const cursor = collection.find(query);
            const list = await cursor.toArray();

            const results = [];

            for (const v of list) {
                results.push({
                    id: v._id.toString(),
                    name: v.name,
                    version: v.version,
                    tags: v.tags,
                    status: v.status,
                    createdAt: v.createdAt,
                });
            }

            return results;
        } finally {
            await client.close();
        }
    }
}

module.exports = { MongoStorage };
