# IHB Platform

Платформа на основе микросервисов. Проект построен на ASP.NET Core 9, использует Kubernetes для оркестрации и **Helm** для управления частями инфраструктуры.

---

## Структура проекта

```
ihb-platform/
├── UserService/           # Микросервис управления пользователями
├── GatewayService/        # API Gateway с JWT аутентификацией (маршрутизация запросов)
├── k8s/                   # Kubernetes манифесты для развёртывания
├── helm/                  # Helm чарты для инфраструктуры
└── postman-collection/    # Postman коллекции для тестирования
```

---

### Стек технологий

- **Framework**: ASP.NET Core 9
- **ORM**: Entity Framework Core с миграциями
- **СУБД**: PostgreSQL
- **Оркестрация**: Kubernetes
- **Package Manager**: Helm

## Развертывание

### Шаг 1: Установка ingress-nginx контроллера

```bash
# Создайте namespace для ingress-nginx
kubectl create namespace ingress-nginx

# Добавьте репозиторий и установите ingress-nginx
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx/
helm repo update

# Установите контроллер
helm install nginx ingress-nginx/ingress-nginx \
  --namespace ingress-nginx \
  -f helm/ingress-nginx-values.yaml
```

### Шаг 2: Установка Prometheus и Grafana

```bash
# Добавьте репозиторий Prometheus
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo update

# Установите kube-prometheus-stack
helm install stack prometheus-community/kube-prometheus-stack \
  -f helm/prometheus-values.yaml
```

#### Настройка доступа к Prometheus и Grafana

```bash
# Создайте Ingress для Prometheus
kubectl apply -f k8s/monitoring/monitoring-prometheus-ingress.yaml

# Создайте Ingress для Grafana
kubectl apply -f k8s/monitoring/monitoring-grafana-ingress.yaml

# Создайте Service для сбора метрик nginx-ingress контроллера
kubectl apply -f k8s/ingress-nginx/ingress-nginx-service.yaml

# Создайте ServiceMonitor для nginx-ingress
kubectl apply -f k8s/ingress-nginx/ingress-nginx-servicemonitor.yaml
```

### Шаг 4: Развёртывание микросервисов

Каждый микросервис содержит свою инструкцию по развёртыванию:

1. **[UserService инструкция по развёртыванию](./UserService/README.md#развёртывание)** - создание БД, миграции, развёртывание сервиса
2. **[GatewayService инструкция по развёртыванию](./GatewayService/README.md#развёртывание)** - настройка шлюза и маршрутизации

## 📊 Мониторинг

### Prometheus

Метрики сервисов собираются в Prometheus: <http://prometheus.arch.homework>

### Grafana

Дашборды для визуализации метрик: <http://grafana.arch.homework>

---
