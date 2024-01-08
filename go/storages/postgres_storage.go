package storages

import (
	"database/sql"
	"github.com/lib/pq"
	"log"
	"time"

	_ "github.com/lib/pq"
)

type PostgresStorage struct {
}

func (PostgresStorage) Init() error {
	pgConnStr := "postgres://postgres:123456@127.0.0.1:5432?sslmode=disable"
	db, err := sql.Open("postgres", pgConnStr)
	if err != nil {
		log.Fatalf("Failed to connect to PostgreSQL: %s", err)
	}
	defer db.Close()

	_, err = db.Exec(`DROP TABLE IF EXISTS projects`)
	if err != nil {
		log.Fatalf("Failed to drop table: %s", err)
	}

	_, err = db.Exec(`
		CREATE TABLE projects (
			id SERIAL PRIMARY KEY,
			name VARCHAR(100) NOT NULL,
			version INTEGER NOT NULL,
			tags TEXT[],
			status BOOLEAN NOT NULL,
		    createdAt timestamp NOT NULL
		)`)
	if err != nil {
		log.Fatalf("Failed to create table: %s", err)
	}

	for _, v := range data {
		_, err = db.Exec("INSERT INTO projects (name, version, tags, status, createdAt) VALUES ($1, $2, $3, $4, $5)", v.name, v.version, pq.Array(v.tags), v.status, v.createdAt)
		if err != nil {
			log.Fatalf("Failed to insert data: %s", err)
		}
	}

	return nil
}

func (PostgresStorage) Query(condition string, args []interface{}) ([]map[string]interface{}, error) {
	pgConnStr := "postgres://postgres:123456@127.0.0.1:5432?sslmode=disable"
	db, err := sql.Open("postgres", pgConnStr)
	if err != nil {
		log.Fatalf("Failed to connect to PostgreSQL: %s", err)
	}
	defer db.Close()

	rows, err := db.Query("SELECT id, name, version, tags, status, createdAt FROM projects WHERE "+condition, args...)
	if err != nil {
		log.Fatalf("Failed to execute query: %s", err)
	}
	defer rows.Close()

	results := make([]map[string]interface{}, 0)
	for rows.Next() {
		var id int
		var name string
		var version int
		var tags []string
		var status bool
		var createdAt time.Time
		if err := rows.Scan(&id, &name, &version, (*pq.StringArray)(&tags), &status, &createdAt); err != nil {
			log.Fatalf("Failed to scan row: %s", err)
		}
		results = append(results, map[string]interface{}{
			"id":        id,
			"name":      name,
			"version":   version,
			"tags":      tags,
			"status":    status,
			"createdAt": createdAt,
		})
	}

	return results, nil
}
