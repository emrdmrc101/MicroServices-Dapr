# MicroServices-Dapr
 Micro Service Dapr

LessonService, ActivityService, IdentityService için bir sidecarProxy oluşturduk.

LessonService > Lesson/Lesson.Infrastructure/Grpc/Services/ActivityService.cs içeriside ActivityService içerisinde gprc  kanalına dapr üzerinden istek atarak örnek oluşturduk.
Dapr prometheus, prometheus > grafanaya bağlıyarak metrik değerleri okuduk.
