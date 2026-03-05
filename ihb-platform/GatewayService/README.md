# GatewayService - API Gateway

**GatewayService** — это центральная точка входа для всех клиентских запросов к микросервисам. Он выполняет маршрутизацию запросов, валидацию JWT токенов и управление аутентификацией.

---

## Описание

GatewayService реализует паттерн API Gateway с использованием **YARP** (Yet Another Reverse Proxy) для:

- **Маршрутизации** запросов к микросервисам на основе путей
- **JWT аутентификации** для защиты endpoints
- **Swagger документации** с поддержкой Bearer токенов
- **Журналирования** запросов и ответов
- **CORS конфигурации** для кросс-доменных запросов

---

## 🔧 Архитектура

### Маршруты

По умолчанию GatewayService маршрутизирует запросы следующим образом:

| Маршрут   | Service     |
| --------- | ----------- |
| `/user/*` | userservice |

---

## Развёртывание в Kubernetes

### Конфигурации

- **`k8s/jwt/jwt-secret.yaml`** — jwt секреты
- **`k8s/gatewayservice/gateway-configmap.yaml`** — публичные параметры конфигурации
- **`k8s/gatewayservice/gateway-deployment.yaml`** — описание развёртывания сервиса
- **`k8s/gatewayservice/gateway-service.yaml`** — K8s Service для внутреннего доступа
- **`k8s/gatewayservice/gateway-ingress.yaml`** — маршрутизация внешнего трафика.

### Предварительные условия

- UserService развёрнут в кластере
- Kubernetes кластер работает
- JWT Secret создан

### Развёртывание сервиса в Kubernetes

Примените все необходимые Kubernetes манифесты в корректном порядке:

```shell

# 1. Создайте ConfigMap с конфигурацией приложения
kubectl apply -f k8s/gateway/gateway-configmap.yaml

# 2. Разверните приложение (Deployment)
kubectl apply -f k8s/gateway/gateway-deployment.yaml

# 3. Создайте Service для доступа к сервису
kubectl apply -f k8s/gateway/gateway-service.yaml

# 4. Настройте Ingress для маршрутизации трафика
kubectl apply -f k8s/gateway/gateway-ingress.yaml

```

---
