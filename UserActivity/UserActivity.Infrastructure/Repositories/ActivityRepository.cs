using MongoDB.Driver;
using UserActivity.Domain.Entities;
using UserActivity.Domain.Interfaces;
using UserActivity.Infrastructure.Objects;

namespace UserActivity.Infrastructure.Repositories;

public class ActivityRepository(IMongoDatabase database, MongoDBSettings mongoDbSettings)
    : Repository<Activities>(database, mongoDbSettings), IActivityRepository;