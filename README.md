# OTUS: Микросервисная архитектура

Этот репозиторий содержит проект курса "Микросервисная архитектура" на платформе OTUS.

## Структура репозитория

- **[ihb-platform-backend](ihb-platform-backend/)** - Основная платформа.[README](ihb-platform-backend/README.md)
  - [UserService](ihb-platform-backend/UserService/) - Микросервис управления пользователями ([README](ihb-platform-backend/UserService/README.md))
  - [GatewayService](ihb-platform-backend/GatewayService/) - API Gateway с аутентификацией ([README](ihb-platform-backend/GatewayService/README.md))

## Технологии

- **ASP.NET Core 9** - Фреймворк для разработки микросервисов
- **Entity Framework Core** - ORM для работы с данными
- **PostgreSQL** - Реляционная база данных
- **Kubernetes** - Платформа оркестрации контейнеров
- **Helm** - Менеджер пакетов для Kubernetes
- **Docker** - Контейнеризация приложений
- **Prometheus & Grafana** - Мониторинг и визуализация метрик
