using System;
using biometrics_app.Data;
using biometrics_app.Interfaces;
using biometrics_app.Models;

namespace biometrics_app.Services;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByIdAsync(int userId) => await _context.Users.FindAsync(userId);

    public async Task<bool> SaveFingerprintDataAsync(int userId, byte[] fingerprintData)
    {
        var userFingerprint = await _context.UserFingerprints.FindAsync(userId);
        if (userFingerprint == null)
        {
            userFingerprint = new UserFingerprint { UserId = userId, FingerprintData = fingerprintData };
            _context.UserFingerprints.Add(userFingerprint);
        }
        else
        {
            userFingerprint.FingerprintData = fingerprintData;
            _context.UserFingerprints.Update(userFingerprint);
        }

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<byte[]?> GetFingerprintDataAsync(int userId)
    {
        var userFingerprint = await _context.UserFingerprints.FindAsync(userId);
        return userFingerprint?.FingerprintData;
    }
}

