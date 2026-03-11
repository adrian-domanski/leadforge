dev:
	docker compose -f docker-compose.dev.yml up

up:
	docker compose up --build

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
