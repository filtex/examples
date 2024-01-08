package main

import (
	"filtex-go-example/storages"
	"fmt"
	"github.com/gorilla/mux"
	"net/http"

	"filtex-go-example/handlers"
)

func main() {
	if err := (storages.MongoStorage{}).Init(); err != nil {
		panic(err)
	}

	if err := (storages.PostgresStorage{}).Init(); err != nil {
		panic(err)
	}

	router := mux.NewRouter()

	router.Use(enableCORS)

	router.HandleFunc("/", homeHandler)
	router.HandleFunc("/metadata", handlers.MetadataHandler)
	router.HandleFunc("/filter/memory", handlers.MemoryFilterHandler)
	router.HandleFunc("/filter/mongo", handlers.MongoFilterHandler)
	router.HandleFunc("/filter/postgres", handlers.PostgresFilterHandler)

	http.Handle("/", router)

	port := 8080
	fmt.Printf("Server is running on http://localhost:%d\n", port)
	err := http.ListenAndServe(fmt.Sprintf(":%d", port), nil)
	if err != nil {
		fmt.Println("Error starting the server:", err)
	}
}

func homeHandler(w http.ResponseWriter, r *http.Request) {
	fmt.Fprintf(w, "Filtex!")
}

func enableCORS(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Access-Control-Allow-Origin", "*")
		w.Header().Set("Access-Control-Allow-Methods", "GET, POST, OPTIONS, PUT, DELETE")
		w.Header().Set("Access-Control-Allow-Headers", "Content-Type, Authorization")

		if r.Method == "OPTIONS" {
			w.WriteHeader(http.StatusOK)
			return
		}

		next.ServeHTTP(w, r)
	})
}
