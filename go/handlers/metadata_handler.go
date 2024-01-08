package handlers

import (
	"encoding/json"
	"net/http"

	"filtex-go-example/filters"
)

func MetadataHandler(w http.ResponseWriter, r *http.Request) {
	projectFilter, err := filters.ProjectFilter()
	if err != nil {
		http.Error(w, "Error generating Filtex instance", http.StatusInternalServerError)
		return
	}

	response, err := projectFilter.Metadata()
	if err != nil {
		http.Error(w, "Error getting Filtex metadata", http.StatusInternalServerError)
		return
	}

	jsonResponse, err := json.Marshal(response)
	if err != nil {
		http.Error(w, "Error encoding JSON response", http.StatusInternalServerError)
		return
	}

	w.Header().Set("Content-Type", "application/json")

	w.Write(jsonResponse)
}
