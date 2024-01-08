const { ProjectFilter } = require('../filters/projectFilter');

class MetadataHandler {
    async handleAsync(req, res) {
        try {
            const response = ProjectFilter.metadata();

            res.status(200).json(response);
        } catch (ex) {
            res.status(500).send(ex.message);
        }
    }
}

module.exports = { MetadataHandler };
