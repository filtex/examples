const {data} = require("./data");

class MemoryStorage {
    Query(fn) {
        const results = [];

        let id = 1;
        for (const v of data) {
            if (fn(v)) {
                results.push({
                    id: id++,
                    name: v.name,
                    version: v.version,
                    tags: v.tags,
                    status: v.status,
                    createdAt: v.createdAt
                });
            }
        }

        return results;
    }
}

module.exports = { MemoryStorage };
