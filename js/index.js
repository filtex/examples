const express = require('express');
const cors = require('cors');

const { MetadataHandler } = require('./handlers/metadataHandler');
const { MemoryFilterHandler } = require('./handlers/memoryFilterHandler');
const { MongoFilterHandler } = require('./handlers/mongoFilterHandler');
const { PostgresFilterHandler } = require('./handlers/postgresFilterHandler');
const {PostgresStorage} = require("./storages/postgresStorage");
const {MongoStorage} = require("./storages/mongoStorage");

const init = async () => {
    await new MongoStorage().Init();
    await new PostgresStorage().Init();
};

init();

const app = express();

app.use(express.json())
app.use(cors());

app.get('/', async (req, res) => {
    res.status(200).send('Filtex!');
});

app.get('/metadata', async (req, res) => {
    await new MetadataHandler().handleAsync(req, res);
});

app.post('/filter/memory', async (req, res) => {
    await new MemoryFilterHandler().handleAsync(req, res);
});

app.post('/filter/mongo', async (req, res) => {
    await new MongoFilterHandler().handleAsync(req, res);
});

app.post('/filter/postgres', async (req, res) => {
    await new PostgresFilterHandler().handleAsync(req, res);
});

const PORT = 8080;

app.listen(PORT, () => {
    console.log(`Server is running on http://localhost:${PORT}`);
});
