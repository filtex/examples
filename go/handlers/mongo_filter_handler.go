package handlers

import (
	"encoding/json"
	"filtex-go-example/filters"
	"filtex-go-example/storages"
	"github.com/filtex/filtex-go/builders/mongo"
	"github.com/filtex/filtex-go/builders/mongo/types"
	"github.com/filtex/filtex-go/expressions"
	"go.mongodb.org/mongo-driver/bson"
	"net/http"
)

func MongoFilterHandler(w http.ResponseWriter, r *http.Request) {
	projectFilter, err := filters.ProjectFilter()
	if err != nil {
		http.Error(w, "Error generating Filtex instance", http.StatusInternalServerError)
		return
	}

	var requestBody map[string]interface{}
	decoder := json.NewDecoder(r.Body)
	err = decoder.Decode(&requestBody)
	if err != nil {
		http.Error(w, "Error decoding JSON request", http.StatusBadRequest)
		return
	}

	var expression expressions.Expression

	switch r.URL.Query().Get("type") {
	case "text":
		if requestBody["query"] == nil {
			break
		}

		query, ok := requestBody["query"].(string)
		if !ok {
			http.Error(w, "Invalid request format", http.StatusBadRequest)
			return
		}

		if query == "" {
			break
		}

		err = projectFilter.ValidateFromText(query)
		if err != nil {
			http.Error(w, "invalid query", http.StatusBadRequest)
			return
		}

		expression, err = projectFilter.ExpressionFromText(query)
		if err != nil {
			http.Error(w, "invalid query", http.StatusBadRequest)
			return
		}
	case "json":
		if requestBody["query"] == nil {
			break
		}

		query, err := json.Marshal(requestBody["query"])
		if err != nil {
			http.Error(w, "Invalid request format", http.StatusBadRequest)
			return
		}

		err = projectFilter.ValidateFromJson(string(query))
		if err != nil {
			http.Error(w, "invalid query", http.StatusBadRequest)
			return
		}

		expression, err = projectFilter.ExpressionFromJson(string(query))
		if err != nil {
			http.Error(w, "invalid query", http.StatusBadRequest)
			return
		}
	default:
		http.Error(w, "invalid type", http.StatusBadRequest)
		return
	}

	var mongoFilter *types.MongoExpression

	if expression != nil {
		mongoFilter, err = mongo.NewMongoFilterBuilder().Build(expression)
		if err != nil {
			http.Error(w, "invalid query", http.StatusBadRequest)
			return
		}
	} else {
		mongoFilter = &types.MongoExpression{
			Condition: bson.M{},
		}
	}

	results, err := storages.MongoStorage{}.Query(mongoFilter.Condition)
	if err != nil {
		http.Error(w, "could not be fetched", http.StatusBadRequest)
		return
	}

	jsonResponse, err := json.Marshal(results)
	if err != nil {
		http.Error(w, "Error encoding JSON response", http.StatusInternalServerError)
		return
	}

	w.Header().Set("Content-Type", "application/json")

	w.Write(jsonResponse)
}
