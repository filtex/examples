const { MemoryFilterBuilder } = require('filtex-js');
const { ProjectFilter } = require('../filters/projectFilter');
const { MemoryStorage } = require('../storages/memoryStorage');
const {MemoryExpression} = require("filtex-js/dist/builders/memory/types/memoryExpression");

class MemoryFilterHandler {
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

            let memoryFilter;

            if (expression) {
                memoryFilter = new MemoryFilterBuilder().build(expression);
            } else {
                memoryFilter = new MemoryExpression((_) => true);
            }

            const results = new MemoryStorage().Query(memoryFilter.fn);

            res.status(200).json(results);
        } catch (ex) {
            res.status(500).send(ex.message);
        }
    }

}

module.exports = { MemoryFilterHandler };
