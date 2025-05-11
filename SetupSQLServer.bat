@echo off

echo.
echo ========================================================
echo  Aplicando YAMLs no Kubernetes...
echo ========================================================
echo.

kubectl apply -f src/Kurbenetes/SQLServer/sqlserver-secret.yaml
kubectl apply -f src/Kurbenetes/SQLServer/sqlserver-pvc.yaml
kubectl apply -f src/Kurbenetes/SQLServer/sqlserver-deployment.yaml
kubectl apply -f src/Kurbenetes/SQLServer/sqlserver-service.yaml

kubectl rollout restart deployment -n healthmed
kubectl rollout restart statefulset -n healthmed

echo.
echo ========================================================
echo  Status dos pods
echo ========================================================
echo.

kubectl get pods -n healthmed

echo.
echo Deploy concluido com sucesso
pause