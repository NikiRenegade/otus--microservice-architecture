# UserService

## Описание

**UserService** - это микросервис, ответственный за управление пользователями. Он реализует полный CRUD функционала для работы с пользователями, интеграцию с ASP.NET Identity для безопасного хранения и управления учётными данными.

Сервис построен на основе архитектуры Clean Architecture с четырьмя слоями:

- **Domain** - бизнес-логика и интерфейсы;
- **Application** - прикладные сервисы и маппинги;
- **Infrastructure** - работа с БД и репозитории;
- **Presentation** - Web API контроллеры.

---

## Основные возможности

### Управление пользователями

- **Регистрация** новых пользователей с хешированием паролей
- **Получение** всех пользователей или одного по ID
- **Поиск** пользователя по `Id` и `Email`
- **Обновление** данных пользователя
- **Удаление** пользователя

### Хранилище данных

- **PostgreSQL** для хранения данных пользователей
- **Entity Framework Core** для ORM
- **Entity Framework Migrations** для управления миграциями
- Интеграция с **Identity Framework**

---

## Web API Endpoints

### GET `/api/user`

Получить всех пользователей.

```json
GET /api/user
Response: 200 OK
[
  { "id": "...", "email": "user@example.com", "firstName": "John", "lastName": "Doe", ... }
]
```

### GET `/api/user/{id}`

Получить пользователя по ID.

```json
GET /api/user/550e8400-e29b-41d4-a716-446655440000
Response: 200 OK / 404 Not Found
```

### GET `/api/user/email/{email}`

Получить пользователя по Email.

```json
GET /api/user/email/user@example.com
Response: 200 OK / 404 Not Found
```

### POST `/api/user`

Создать нового пользователя.

```json
POST /api/user
Content-Type: application/json

{
  "email": "newuser@example.com",
  "password": "SecurePassword123!",
  "userName": "newuser",
  "firstName": "John",
  "lastName": "Doe"
}

Response: 201 Created
{
  "id": "...",
  "email": "newuser@example.com",
  "userName": "newuser",
  "firstName": "John",
  "lastName": "Doe"
}
```

### PUT `/api/user/{id}`

Обновить данные пользователя.

```json
PUT /api/user/550e8400-e29b-41d4-a716-446655440000
Content-Type: application/json

{
  "email": "updated@example.com",
  "userName": "updateduser",
  "firstName": "Jane",
  "lastName": "Smith"
}

Response: 204 No Content / 404 Not Found
```

### DELETE `/api/user/{id}`

Удалить пользователя.

```json
DELETE /api/user/550e8400-e29b-41d4-a716-446655440000
Response: 204 No Content / 404 Not Found
```

---

## Развёртывание

- **Kubernetes** манифесты для оркестрации контейнеров
- **Helm** чарты для развёртывания зависимостей (PostgreSQL)
- **Docker** поддержка контейнеризации
- Jobs для запуска миграций БД

### Конфигурации

- **`helm/values.yaml`** — параметры Helm чарта для PostgreSQL
- **`k8s/userservice/userservice-secret.yaml`** — учётные данные и переменные
- **`k8s/userservice/userservice-configmap.yaml`** — публичные параметры конфигурации
- **`k8s/userservice/userservice-deployment.yaml`** — описание развёртывания сервиса
- **`k8s/userservice/userservice-service.yaml`** — K8s Service для внутреннего доступа
- **`k8s/userservice/userservice-ingress.yaml`** — маршрутизация внешнего трафика

### Установка базы данных через Helm

Добавьте репозиторий Bitnami и установите PostgreSQL:

```shell
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update
helm install users-postgres bitnami/postgresql -f helm/values.yaml
```

Этот шаг создаёт PostgreSQL с конфигурацией из `helm/values.yaml`.

### Развёртывание сервиса в Kubernetes

Примените все необходимые Kubernetes манифесты в корректном порядке:

```shell
# 1. Создайте Secret с переменными окружения и учётными данными
kubectl apply -f k8s/userservice/userservice-secret.yaml

# 2. Создайте ConfigMap с конфигурацией приложения
kubectl apply -f k8s/userservice/userservice-configmap.yaml

# 3. Запустите Job для миграций БД
kubectl apply -f k8s/jobs/userservice-migration-job.yaml

# Необходимо дождаться завершения миграций. Для проверки:
kubectl get jobs userservice-migration-job -w

# 4. Разверните приложение (Deployment)
kubectl apply -f k8s/userservice/userservice-deployment.yaml

# 5. Создайте Service для доступа к сервису
kubectl apply -f k8s/userservice/userservice-service.yaml

# 6. Настройте Ingress для маршрутизации трафика
kubectl apply -f k8s/userservice/userservice-ingress.yaml
```

---

## Тестирование

Используйте приложенную Postman коллекцию для тестирования API:

```sh
UserService.Presentation.API.postman_collection.json
```

Импортируйте этот файл в Postman и запустите тесты на вашем развёрнутом сервисе.

---
