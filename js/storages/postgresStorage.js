const { Pool } = require('pg');
const format = require('pg-format');
const {data} = require("./data");

class PostgresStorage {
    constructor() {
        this.pool = new Pool({
            user: 'postgres',
            password: '123456',
            host: '127.0.0.1',
            port: 5432,
            database: 'postgres',
        });
    }

    async Init() {
        const client = await this.pool.connect();

        await client.query('DROP TABLE IF EXISTS projects;');

        await client.query(`
            CREATE TABLE projects (
              id SERIAL PRIMARY KEY,
              name VARCHAR(100) NOT NULL,
              version INTEGER NOT NULL,
              tags TEXT[],
              status BOOLEAN NOT NULL,
              createdAt TIMESTAMP NOT NULL
            );`);

        for (let v of data)
        {
            const query = format('INSERT INTO projects (name, version, tags, status, createdAt) VALUES (%L, %L, ARRAY[%L], %L, %L)', v.name, v.version, v.tags, v.status, v.createdAt);
            await client.query(query, []);
        }
    }

    async Query(condition, args) {
        const client = await this.pool.connect();

        const queryText = `SELECT id, name, version, tags, status, createdAt FROM projects WHERE ${condition}`;
        const { rows } = await client.query(queryText, args);

        client.release();
        await this.pool.end();

        return rows.map((row) => ({
            id: row.id,
            name: row.name,
            version: row.version,
            tags: row.tags,
            status: row.status,
            createdAt: row.createdAt,
        }));
    }
}

module.exports = { PostgresStorage };
