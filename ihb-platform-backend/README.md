# IHB Platform

Платформа на основе микросервисов. Проект построен на ASP.NET Core 9. Используется Kubernetes для оркестрации и Helm для управления частями инфраструктуры.

## Структура проекта

```
ihb-platform-backend/
├── UserService/                    # Микросервис управления пользователями
├── DeliveryService/                 # Микросервис управления биллингом
├── InventoryService/                 # Микросервис управления доставкой
├── BillingService/                 # Микросервис управления складом
├── OrderService/                   # Микросервис управления заказами
├── NotificationService/            # Микросервис отправки уведомлений
├── GatewayService/                 # API Gateway (точка входа)
├── Shared/                         # Общие библиотеки
├── helm/                           # Helm чарты для развёртывания
├── k8s/                            # Kubernetes манифесты
└── postman-collection/             # Postman коллекции для тестирования
```

---

## Стек технологий

| Компонент                   | Технология                       |
| --------------------------- | -------------------------------- |
| **Framework**               | ASP.NET Core 9                   |
| **Language Runtime**        | .NET 9                           |
| **ORM**                     | Entity Framework Core            |
| **БД**                      | PostgreSQL                       |
| **Message Broker**          | RabbitMQ                         |
| **Оркестрация**             | Kubernetes                       |
| **Package Manager для K8s** | Helm                             |
| **Мониторинг**              | Prometheus + Grafana             |
| **Ingress Controller**      | ingress-nginx                    |
| **API Gateway**             | YARP (Yet Another Reverse Proxy) |
| **Аутентификация**          | JWT Bearer tokens                |

## Процесс развёртывания

Для разворачивания платформы скопируйте папки k8s и helm на ВМ.

### Этап 0: Подготовка окружения

Перейдите в папку в которую было скопированы k8s и helm

```bash
cd ...
```

### Этап 1: Подготовка окружения

```bash
# Создайте namespace для ingress-nginx
kubectl create namespace ihb-platform
```

### Этап 2: Установка системных компонентов

#### 2.1 Установка ingress-nginx

```bash
# Добавьте репозиторий
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update

# Установите ingress-nginx
helm install nginx ingress-nginx/ingress-nginx \
  --namespace ingress-nginx \
  -f helm/ingress-nginx-values.yaml
```

#### 2.2 Установка Prometheus + Grafana (мониторинг)

```bash
# Добавьте репозиторий
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo update

# Установите kube-prometheus-stack
helm install stack prometheus-community/kube-prometheus-stack \
  -f helm/prometheus-values.yaml
```

#### 2.3 Конфигурация доступа к Prometheus и Grafana

```bash
# Создайте Ingress для Prometheus
kubectl apply -f k8s/monitoring/monitoring-prometheus-ingress.yaml

# Создайте Ingress для Grafana
kubectl apply -f k8s/monitoring/monitoring-grafana-ingress.yaml

# Создайте Service и ServiceMonitor для сбора метрик nginx
kubectl apply -f k8s/ingress-nginx/ingress-nginx-service.yaml
kubectl apply -f k8s/ingress-nginx/ingress-nginx-servicemonitor.yaml
```

#### 2.4 Установка RabbitMQ (message broker)

```bash
# Установите оператор
kubectl apply -f "https://github.com/rabbitmq/cluster-operator/releases/latest/download/cluster-operator.yml"

# Развертывание RabbitMQ в кластере
kubectl apply -f k8s/rabbitmq/rabbitmq.yaml

# Создание Ingress для доступа к RabbitMQ Management UI
kubectl apply -f k8s/rabbitmq/rabbitmq-ingress.yaml
```

#### 2.6 Установка Redis

```bash
# Установка Redis
helm install redis bitnami/redis -f helm/redis-values.yaml
```

### Этап 3: Развёртывание PostgreSQL для каждого сервиса

```bash
# Добавьте репозиторий Bitnami
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update

# Установите PostgreSQL для UserService
helm install users-db bitnami/postgresql \
  -f helm/db/usersdb-postgres-values.yaml

# Установите PostgreSQL для OrderService
helm install orders-db bitnami/postgresql \
  -f helm/db/ordersdb-postgres-values.yaml

# Установите PostgreSQL для BillingService
helm install billings-db bitnami/postgresql \
  -f helm/db/billingsdb-postgres-values.yaml
  
# Установите PostgreSQL для DeliveryService
helm install deliveries-db bitnami/postgresql \
  -f helm/db/deliveriesdb-postgres-values.yaml
  
# Установите PostgreSQL для InventoryService
helm install inventories-db bitnami/postgresql \
  -f helm/db/inventoriesdb-postgres-values.yaml

# Установите PostgreSQL для NotificationService
helm install notifications-db bitnami/postgresql \
  -f helm/db/notoficationsdb-postgres-values.yaml
```

### Этап 4: Развёртывание configmap и secrets

```bash
# Разверните confiigmap для GatewayService
kubectl apply -f k8s/configMaps/gateway-configmap.yaml

# Разверните secret (строка подключения к бд) для UserService
kubectl apply -f k8s/secrets/userservice-secret.yaml

# Разверните secret (строка подключения к бд) для OrderService
kubectl apply -f k8s/secrets/orderservice-secret.yaml

# Разверните secret (строка подключения к бд) для BillingService
kubectl apply -f k8s/secrets/billingservice-secret.yaml

# Разверните secret (строка подключения к бд) для DeliveyService
kubectl apply -f k8s/secrets/deliveryservice-secret.yaml

# Разверните secret (строка подключения к бд) для InventoryService
kubectl apply -f k8s/secrets/inventoryservice-secret.yaml

# Разверните secret (строка подключения к бд) для NotificationService
kubectl apply -f k8s/secrets/notificationservice-secret.yaml
```

### Этап 5: Развёртывание микросервисов

Все микросервисы разворачиваются через один Helm chart:

```bash
# Разверните микросервисы
helm install ihb-platform ./helm/ihb-chart


# Проверьте статус всех сервисов
kubectl get pods
kubectl get svc
```

### Этап 6: Конфигурация hosts (для использования доменов)

Добавьте в `/etc/hosts` (macOS/Linux):

```
<ip> ihb-platform.local
<ip> prometheus.arch.homework
<ip> grafana.arch.homework
<ip> rabbitmq.arch.homework
```

## Микросервисы

### UserService

**Описание**: Управление пользователями и аутентификация

**Функционал**:

- Регистрация новых пользователей
- Логин и выдача JWT токенов
- CRUD операции с профилем пользователя
- Хеширование паролей (Bcrypt/PBKDF2)
- Отправка уведомлений о создании пользователя через RabbitMQ

**Технологии**: ASP.NET Core 9, Entity Framework Core, PostgreSQL, ASP.NET Identity, RabbitMq

### BillingService

**Описание**: Управление биллингом и платежами

**Функционал**:

- Обработка событий создания пользователя из RabbitMQ
- Создание и управление счетами

**Технологии**: ASP.NET Core 9, Entity Framework Core, PostgreSQL, RabbitMQ

### DeliveryService

**Описание**: Управление доставкой

**Функционал**:

- Создание и управление слотами доставки

**Технологии**: ASP.NET Core 9, Entity Framework Core, PostgreSQL

### InventoryService

**Описание**: Управление товаром на складе

**Функционал**:

- Создание и управление товарами на складе

**Технологии**: ASP.NET Core 9, Entity Framework Core, PostgreSQL


### OrderService

**Описание**: Управление заказами

**Функционал**:

- Создание заказов
- Изменение статуса заказа
- Интеграция с BillingService (http)
- Отправка уведомлений через RabbitMQ

**Технологии**: ASP.NET Core 9, Entity Framework Core, PostgreSQL, RabbitMQ

### NotificationService

**Описание**: Система уведомлений

**Функционал**:

- Обработка событий из RabbitMQ
- Хранение истории уведомлений

**Технологии**: ASP.NET Core 9, Entity Framework Core, PostgreSQL, RabbitMQ

### GatewayService

**Описание**: API Gateway (единая точка входа)

**Функционал**:

- Маршрутизация запросов к микросервисам
- Валидация JWT токенов

**Маршруты**:

| Путь              | Целевой сервис      |
|-------------------|---------------------|
| `/user/*`         | UserService         |
| `/order/*`        | OrderService        |
| `/billing/*`      | BillingService      |
| `/delivery/*`     | DeliveryService     |
| `/inventory/*`    | InventoryService    |
| `/notification/*` | NotificationService |

**Технологии**: ASP.NET Core 9, YARP, JWT Bearer

## Мониторинг

### Prometheus

Собирает метрики со всех сервисов:

- Доступ: <http://prometheus.arch.homework>
- Метрики доступны на `/metrics` в каждом сервисе

### Grafana

Визуализация метрик и дашборды:

- Доступ: <http://grafana.arch.homework>
- Стандартные учётные данные обычно: admin/admin

### RabbitMQ Management UI

Управление message broker:

- Доступ: <http://rabbitmq.arch.homework>
- Данные хранятся в секрете rabbitmq-default-user
