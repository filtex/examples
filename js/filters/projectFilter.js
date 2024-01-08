const { Filtex, FieldOption, LookupOption, Lookup } = require('filtex-js');

const ProjectFilter = Filtex.new(
    FieldOption.new().string().name("name").label("Name").nullable(),
    FieldOption.new().number().name("version").label("Version"),
    FieldOption.new().string().array().name("tags").label("Tags").nullable(),
    FieldOption.new().boolean().name("status").label("Status").lookup("statuses"),
    FieldOption.new().datetime().name("createdAt").label("Created At"),
    LookupOption.new().key("statuses").values([
        new Lookup("Enabled", true),
        new Lookup("Disabled", false),
    ]),
);

module.exports = { ProjectFilter };
