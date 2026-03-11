up:
	docker compose up --build

postgres:
	docker compose up -d postgres

down:
	docker compose down

logs:
	docker compose logs -f

build:
	docker compose build

reset-db:
	docker compose down -v

clean:
	docker system prune -f
