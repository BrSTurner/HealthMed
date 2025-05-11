@echo off

echo.
echo ========================================================
echo  Aplicando YAMLs no Kubernetes...
echo ========================================================
echo.

kubectl apply -f src/Kurbenetes/namespace.yaml

kubectl apply -f src/Kurbenetes/Services/services-configmap.yaml
kubectl apply -f src/Kurbenetes/Services/services-secret.yaml

REM kubectl apply -f src/Kurbenetes/PostgreSQL/postgresql-configmap.yaml
REM kubectl apply -f src/Kurbenetes/PostgreSQL/postgresql-secret.yaml
REM kubectl apply -f src/Kurbenetes/PostgreSQL/postgresql-pv.yaml
REM kubectl apply -f src/Kurbenetes/PostgreSQL/postgresql-pvc.yaml
REM kubectl apply -f src/Kurbenetes/PostgreSQL/postgresql-deployment.yaml
REM kubectl apply -f src/Kurbenetes/PostgreSQL/postgresql-service.yaml

kubectl apply -f src/Kurbenetes/RabbitMQ/rabbitmq-deployment.yaml
kubectl apply -f src/Kurbenetes/RabbitMQ/rabbitmq-service.yaml


kubectl apply -f src/UserService/Med.User.WebAPI/user-webapi-deployment.yaml
kubectl apply -f src/UserService/Med.User.WebAPI/user-webapi-service.yaml

kubectl apply -f src/CalendarService/Med.Calendar.WebAPI/calendar-webapi-deployment.yaml
kubectl apply -f src/CalendarService/Med.Calendar.WebAPI/calendar-webapi-service.yaml

kubectl apply -f src/AuthenticationService/Med.Authentication.WebAPI/auth-webapi-deployment.yaml
kubectl apply -f src/AuthenticationService/Med.Authentication.WebAPI/auth-webapi-service.yaml

kubectl apply -f src/AppointmentService/Med.Appointment.WebAPI/appointment-webapi-deployment.yaml
kubectl apply -f src/AppointmentService/Med.Appointment.WebAPI/appointment-webapi-service.yaml

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