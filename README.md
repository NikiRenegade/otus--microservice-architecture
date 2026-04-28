# OTUS: Микросервисная архитектура

Этот репозиторий содержит проект курса "Микросервисная архитектура" на платформе OTUS.

## Структура репозитория

- **[ihb-platform-backend](ihb-platform-backend/)** - Основная платформа ([README](ihb-platform-backend/README.md))
  - [UserService](ihb-platform-backend/UserService/) - Микросервис управления пользователями
  - [OrderService](ihb-platform-backend/UserService/) - Микросервис управления заказами
  - [BillingService](ihb-platform-backend/UserService/) - Микросервис управления платежами
  - [DeliveryService](ihb-platform-backend/UserService/) - Микросервис управления доставкой
  - [InventoryService](ihb-platform-backend/UserService/) - Микросервис управления товаром на складе
  - [NotificationService](ihb-platform-backend/UserService/) - Микросервис управления уведомлениями
  - [GatewayService](ihb-platform-backend/GatewayService/) - API Gateway с аутентификацией

## Технологии

- **ASP.NET Core 9** - Фреймворк для разработки микросервисов
- **Entity Framework Core** - ORM для работы с данными
- **PostgreSQL** - Реляционная база данных
- **RabbitMq** - Обмен сообщениями между сервисами
- **Kubernetes** - Платформа оркестрации контейнеров
- **Helm** - Менеджер пакетов для Kubernetes
- **Docker** - Контейнеризация приложений
- **Prometheus & Grafana** - Мониторинг и визуализация метрик
