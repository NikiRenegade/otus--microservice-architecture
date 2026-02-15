# Инструкция по запуску приложения.

1. Для запуска приложения требуются 2 дирректории: helm для развертывания БД и k8s;
2. Для установки БД через helm необходимо ввести следующие команды:

```shell
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update
helm install users-db bitnami/postgresql -f helm/values.yaml
```

3. Для запуска приложения требуется ввести следующие команды:

```shell
kubectl apply -f k8s/userservice/userservice-secret.yaml
kubectl apply -f k8s/userservice/userservice-configmap.yaml
kubectl apply -f k8s/jobs/userservice-migration-job.yaml
kubectl apply -f k8s/userservice/userservice-deployment.yaml
kubectl apply -f k8s/userservice/userservice-service.yaml
kubectl apply -f k8s/userservice/userservice-ingress.yaml
```