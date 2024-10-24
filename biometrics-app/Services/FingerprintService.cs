using biometrics_app.Interfaces;
using DPUruNet;

namespace biometrics_app.Services
{
    public class FingerprintService : IFingerprintService
    {
        private readonly Reader _reader;
        private readonly ReaderCollection _readers = ReaderCollection.GetReaders();
        private Fmd _firstFinger;

        public FingerprintService()
        {
            _reader = _readers[0];
        }


        public Task<byte[]> CaptureFingerprintAsync(int userId)
        {
            // This method captures the fingerprint using the Digital Persona SDK
            // Capture the fingerprint data asynchronously
            byte[]? fingerprintData = [];
            if (!OpenReader())
            {
                return Task.FromResult(fingerprintData);
            }
            if (!StartCaptureAsync(this.EnrollFingerprintAsync))
            {
                fingerprintData = _firstFinger.Bytes;
            }
            return Task.FromResult(fingerprintData);
        }
        public void EnrollFingerprintAsync(CaptureResult captureResult)
        {
            try
            {
                if (!CheckCaptureResult(captureResult)) return;

                DataResult<Fmd> resultConversion = FeatureExtraction.CreateFmdFromFid(captureResult.Data, Constants.Formats.Fmd.ANSI);

                if (resultConversion.ResultCode != Constants.ResultCode.DP_SUCCESS)
                {
                    throw new Exception(resultConversion.ResultCode.ToString());
                }

                _firstFinger = resultConversion.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during fingerprint enrollment: " + ex.Message);
            }
        }
        public Task<bool> VerifyFingerprintAsync(byte[] storedFingerprintData, byte[] currentFingerprintData)
        {
            throw new NotImplementedException();
        }

        // Method for capturing fingerprint from the reader
        // Method for capturing fingerprint
        private bool CaptureFingerprint()
        {
            try
            {
                GetStatus();

                Constants.ResultCode captureResult = _reader.CaptureAsync(Constants.Formats.Fid.ANSI, Constants.CaptureProcessing.DP_IMG_PROC_DEFAULT, _reader.Capabilities.Resolutions[0]);
                if (captureResult != Constants.ResultCode.DP_SUCCESS)
                {
                    throw new Exception("" + captureResult);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void GetStatus()
        {
            Constants.ResultCode result = _reader.GetStatus();

            if ((result != Constants.ResultCode.DP_SUCCESS))
            {
                throw new Exception("" + result);
            }

            if (_reader.Status.Status == Constants.ReaderStatuses.DP_STATUS_BUSY)
            {
                Thread.Sleep(50);
            }
            else if (_reader.Status.Status == Constants.ReaderStatuses.DP_STATUS_NEED_CALIBRATION)
            {
                _reader.Calibrate();
            }
            else if (_reader.Status.Status != Constants.ReaderStatuses.DP_STATUS_READY)
            {
                throw new Exception("Reader Status - " + _reader.Status.Status);
            }

        }
        public void CancelCaptureAndCloseReader(Reader.CaptureCallback onCaptured)
        {
            _reader.CancelCapture();
            _reader.Dispose();
        }
        public bool CheckCaptureResult(CaptureResult captureResult)
        {
            if (captureResult.Data == null || captureResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
            {
                if (captureResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
                {
                    throw new Exception(captureResult.ResultCode.ToString());
                }

                if (captureResult.Quality != Constants.CaptureQuality.DP_QUALITY_CANCELED)
                {
                    throw new Exception("Quality - " + captureResult.Quality);
                }
                return false;
            }

            return true;
        }
        public bool OpenReader()
        {

            Constants.ResultCode result = Constants.ResultCode.DP_DEVICE_FAILURE;

            // Open reader
            result = _reader.Open(Constants.CapturePriority.DP_PRIORITY_COOPERATIVE);

            if (result != Constants.ResultCode.DP_SUCCESS)
            {
                return false;
            }
            return true;
        }
        public bool StartCaptureAsync(Reader.CaptureCallback onCaptured)
        {
            // Activate capture handler
            _reader.On_Captured += onCaptured;

            // Call capture
            if (!CaptureFingerprint())
            {
                return false;
            }

            return true;
        }
        

    }

}
