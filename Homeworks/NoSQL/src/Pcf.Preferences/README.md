# Redis Preference Dictionary Service, based on OM .NET Skeleton ASP.NET Core App


### Up databases
docker-compose up promocode-factory-administration-db promocode-factory-receiving-from-partner-db promocode-factory-giving-to-customer-db promocode-factory-preference-db

###only redis
docker run -p 6379:6379 -p 8001:8001 redis/redis-stack
