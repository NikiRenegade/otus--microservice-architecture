# UserService

## Описание

**UserService** — это микросервис, ответственный за управление пользователями и аутентификацию. Он реализует полный CRUD функционал для работы с пользователями, интеграцию с ASP.NET Identity для безопасного хранения и управления учётными данными.

Сервис построен на основе архитектуры Clean Architecture с четырьмя слоями:

- **Domain** - бизнес-логика и интерфейсы;
- **Application** - прикладные сервисы и маппинги;
- **Infrastructure** - работа с БД и репозитории;
- **Presentation** - Web API контроллеры.

---

## Основные возможности

### Управление пользователями

- **Регистрация** новых пользователей с хешированием паролей
- **Логин** пользователя
- **Получение** информации о профиле пользователя по jwt
- **Обновление** информации о профиле пользователя по jwt
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

## Развёртывание

- **Kubernetes** манифесты для оркестрации контейнеров
- **Helm** чарты для развёртывания зависимостей (PostgreSQL)
- **Docker** поддержка контейнеризации
- Jobs для запуска миграций БД

### Конфигурации

- **`helm/postgres-values.yaml`** — параметры Helm чарта для PostgreSQL
- **`k8s/jwt/jwt-secret.yaml`** — jwt секреты
- **`k8s/userservice/userservice-secret.yaml`** — учётные данные и переменные
- **`k8s/userservice/userservice-configmap.yaml`** — публичные параметры конфигурации
- **`k8s/userservice/userservice-deployment.yaml`** — описание развёртывания сервиса
- **`k8s/userservice/userservice-service.yaml`** — K8s Service для внутреннего доступа
- **`userservice-servicemonitor.yaml`** — ServiceMonitor для сбора метрик приложения.

### Установка базы данных через Helm

Добавьте репозиторий Bitnami и установите PostgreSQL:

```shell
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update
helm install users-postgres bitnami/postgresql -f helm/values.yaml
```

Этот шаг создаёт PostgreSQL с конфигурацией из `helm/postgres-values.yaml`.

### Развёртывание сервиса в Kubernetes

Примените все необходимые Kubernetes манифесты в корректном порядке:

```shell

# 0. Создайте jwt Secret
kubectl apply -f k8s/jwt/jwt-secret.yaml

# 1. Создайте Secret с переменными окружения и учётными данными
kubectl apply -f k8s/userservice/userservice-secret.yaml

# 2. Создайте ConfigMap с конфигурацией приложения
kubectl apply -f k8s/userservice/userservice-configmap.yaml

# 3. Запустите Job для миграций БД
kubectl apply -f k8s/userservice-migration-job.yaml

# Необходимо дождаться завершения миграций. Для проверки:
kubectl get jobs userservice-migration-job -w

# 4. Разверните приложение (Deployment)
kubectl apply -f k8s/userservice/userservice-deployment.yaml

# 5. Создайте Service для доступа к сервису
kubectl apply -f k8s/userservice/userservice-service.yaml


# 6. Создайте ServiceMonitor для сбора метрик самого приложения
kubectl apply -f k8s/userservice/userservice-servicemonitor.yaml


---
```
