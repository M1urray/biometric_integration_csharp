using System;
using biometrics_app.Models;

namespace biometrics_app.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int userId);
    Task<bool> SaveFingerprintDataAsync(int userId, byte[] fingerprintData);
    Task<byte[]?> GetFingerprintDataAsync(int userId);
}
