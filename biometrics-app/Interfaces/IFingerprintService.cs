using System;

namespace biometrics_app.Interfaces;

public interface IFingerprintService
{
    Task<byte[]> CaptureFingerprintAsync(int userId);
    Task<bool> VerifyFingerprintAsync(byte[] storedFingerprintData, byte[] currentFingerprintData);
}