package storages

type MemoryStorage struct {
}

func (MemoryStorage) Query(fn func(map[string]interface{}) bool) ([]map[string]interface{}, error) {
	results := make([]map[string]interface{}, 0)
	for id, v := range data {
		if fn(map[string]interface{}{
			"name":      v.name,
			"version":   v.version,
			"tags":      v.tags,
			"status":    v.status,
			"createdAt": v.createdAt,
		}) {
			results = append(results, map[string]interface{}{
				"id":        id + 1,
				"name":      v.name,
				"version":   v.version,
				"tags":      v.tags,
				"status":    v.status,
				"createdAt": v.createdAt,
			})
		}
	}

	return results, nil
}
