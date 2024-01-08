package storages

import "time"

type model struct {
	id        int
	name      string
	version   int
	tags      []string
	status    bool
	createdAt time.Time
}

var data = []model{
	{
		name:    "Filtex GO",
		version: 1,
		tags: []string{
			"filtex",
			"go",
			"backend",
		},
		status:    true,
		createdAt: time.Now().Add(-5 * 24 * time.Hour),
	},
	{
		name:    "Filtex JS",
		version: 1,
		tags: []string{
			"filtex",
			"js",
			"backend",
		},
		status:    true,
		createdAt: time.Now().Add(-4 * 24 * time.Hour),
	},
	{
		name:    "Filtex NET",
		version: 1,
		tags: []string{
			"filtex",
			"net",
			"backend",
		},
		status:    true,
		createdAt: time.Now().Add(-3 * 24 * time.Hour),
	},
	{
		name:    "Filtex UI",
		version: 1,
		tags: []string{
			"filtex",
			"js",
			"frontend",
		},
		status:    true,
		createdAt: time.Now().Add(-2 * 24 * time.Hour),
	},
	{
		name:    "Filtex RUST",
		version: 0,
		tags: []string{
			"filtex",
			"rust",
			"backend",
		},
		status:    false,
		createdAt: time.Now().Add(-1 * 24 * time.Hour),
	},
	{
		name:    "Filtex JAVA",
		version: 0,
		tags: []string{
			"filtex",
			"java",
			"backend",
		},
		status:    false,
		createdAt: time.Now().Add(0 * 24 * time.Hour),
	},
}
