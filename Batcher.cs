using System;
using System.Collections.Generic;

namespace KG_6
{
    internal class Batcher : IDisposable
    {
        private Dictionary<(int, int), Batch> _batches = new Dictionary<(int, int), Batch>();
        private bool _disposedValue;


        public Batch GetBatch(int mesh, int material)
        {
            if(_batches.ContainsKey((mesh, material)))
            {
                return _batches[(mesh, material)];
            }

            Batch batch = new Batch(mesh, material);

            _batches.Add((mesh, material), batch);

            return batch;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (Batch b in _batches.Values)
                        b.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Batcher()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
 