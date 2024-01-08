const { PostgresFilterBuilder } = require('filtex-js');
const { ProjectFilter } = require('../filters/projectFilter');
const { PostgresStorage } = require('../storages/postgresStorage');
const { PostgresExpression } = require("filtex-js/dist/builders/postgres/types/postgresExpression");

class PostgresFilterHandler {
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

            let postgresFilter;

            if (expression) {
                postgresFilter = new PostgresFilterBuilder().build(expression);
            } else {
                postgresFilter = new PostgresExpression('1 = 1', []);
            }

            const results = await new PostgresStorage().Query(postgresFilter.condition, postgresFilter.args);

            res.status(200).json(results);
        } catch (ex) {
            res.status(500).send(ex.message);
        }
    }
}

module.exports = { PostgresFilterHandler };
