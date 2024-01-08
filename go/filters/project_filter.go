package filters

import (
	"github.com/filtex/filtex-go"
	"github.com/filtex/filtex-go/models"
	"github.com/filtex/filtex-go/options"
)

var instance *filtex.Filtex

func ProjectFilter() (*filtex.Filtex, error) {
	if instance != nil {
		return instance, nil
	}

	var err error

	instance, err = filtex.New(
		options.NewFieldOption().String().Name("name").Label("Name").Nullable(),
		options.NewFieldOption().Number().Name("version").Label("Version"),
		options.NewFieldOption().String().Array().Name("tags").Label("Tags").Nullable(),
		options.NewFieldOption().Boolean().Name("status").Label("Status").Lookup("statuses"),
		options.NewFieldOption().DateTime().Name("createdAt").Label("Created At"),
		options.NewLookupOption().Key("statuses").Values([]models.Lookup{
			{"Enabled", true},
			{"Disabled", false},
		}),
	)

	return instance, err
}
