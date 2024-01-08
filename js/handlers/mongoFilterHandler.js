const { MongoFilterBuilder } = require('filtex-js');
const { ProjectFilter } = require('../filters/projectFilter');
const { MongoStorage } = require('../storages/mongoStorage');
const {MongoExpression} = require("filtex-js/dist/builders/mongo/types/mongoExpression");

class MongoFilterHandler {
    async handleAsync(req, res) {
        try {
            let expression;

            switch (req.query.type) {
                case 'text':
                    if (!req.body.query || req.body.query === '') {
                        break;
                    }

                    ProjectFilter.validateFromText(req.body.query);
                    expression = ProjectFilter.expressionFromText(req.body.query);
                    break;
                case 'json':
                    if (!req.body.query) {
                        break;
                    }

                    ProjectFilter.validateFromJson(JSON.stringify(req.body.query));
                    expression = ProjectFilter.expressionFromJson(JSON.stringify(req.body.query));
                    break;
                default:
                    throw new Error('invalid type');
            }

            let mongoFilter;

            if (expression) {
                mongoFilter = new MongoFilterBuilder().build(expression);
            } else {
                mongoFilter = new MongoExpression({});
            }

            const results = await new MongoStorage().Query(mongoFilter.condition);

            res.status(200).json(results);
        } catch (ex) {
            res.status(500).send(ex.message);
        }
    }
}

module.exports = { MongoFilterHandler };
