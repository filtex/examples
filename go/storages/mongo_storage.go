package storages

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
)

type MongoStorage struct {
}

func (MongoStorage) Init() error {
	mongoURI := "mongodb://127.0.0.1:27017"
	client, err := mongo.Connect(context.Background(), options.Client().ApplyURI(mongoURI))
	if err != nil {
		return err
	}
	defer client.Disconnect(context.Background())

	database := client.Database("filtex")
	collection := database.Collection("projects")

	collection.Drop(context.Background())

	docs := make([]interface{}, 0)

	for _, v := range data {
		docs = append(docs, map[string]interface{}{
			"name":      v.name,
			"version":   v.version,
			"tags":      v.tags,
			"status":    v.status,
			"createdAt": v.createdAt,
		})
	}

	_, err = collection.InsertMany(context.Background(), docs)
	if err != nil {
		return err
	}

	return nil
}

func (MongoStorage) Query(query bson.M) ([]interface{}, error) {
	mongoURI := "mongodb://127.0.0.1:27017"
	client, err := mongo.Connect(context.Background(), options.Client().ApplyURI(mongoURI))
	if err != nil {
		return nil, err
	}
	defer client.Disconnect(context.Background())

	database := client.Database("filtex")
	collection := database.Collection("projects")

	cursor, err := collection.Find(context.Background(), query)
	if err != nil {
		return nil, err
	}
	defer cursor.Close(context.Background())

	results := make([]bson.M, 0)
	if err := cursor.All(context.Background(), &results); err != nil {
		return nil, err
	}

	mapped := make([]interface{}, 0)

	for _, v := range results {
		mapped = append(mapped, map[string]interface{}{
			"id":        v["_id"],
			"name":      v["name"],
			"version":   v["version"],
			"tags":      v["tags"],
			"status":    v["status"],
			"createdAt": v["createdAt"],
		})
	}

	return mapped, nil
}
