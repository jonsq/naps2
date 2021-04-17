﻿using System;
using System.Collections.Generic;
using System.Linq;
using NAPS2.Testing;

namespace NAPS2.Ocr
{
    public class OcrEngineManager
    {
        private static OcrEngineManager _default = new OcrEngineManager();

        public static OcrEngineManager Default
        {
            get
            {
                TestingContext.NoStaticDefaults();
                return _default;
            }
            set => _default = value ?? throw new ArgumentNullException(nameof(value));
        }

        private readonly List<IOcrEngine> _engines;

        /// <summary>
        /// Creates a new instance of OcrEngineManager that only looks for Tesseract on the system path.
        /// </summary>
        public OcrEngineManager()
        {
            _engines = new List<IOcrEngine>
            {
                new TesseractSystemEngine()
            };
        }

        /// <summary>
        /// Creates a new instance of OcrEngineManager with the specified engines. The order of engines is important; preferred/newer first.
        /// </summary>
        /// <param name="orderedEngineList"></param>
        public OcrEngineManager(IEnumerable<IOcrEngine> orderedEngineList)
        {
            _engines = orderedEngineList.ToList();
        }

        /// <summary>
        /// Creates a new instance of OcrEngineManager with the default set of engines.
        /// <param name="basePath">The base path for installed engines.</param>
        /// </summary>
        public OcrEngineManager(string basePath)
        {
            _engines = new List<IOcrEngine>
            {
                new Tesseract400Beta4Engine(basePath),
                new Tesseract304Engine(basePath),
                new Tesseract304XpEngine(basePath),
                new Tesseract302Engine(basePath),
                new TesseractSystemEngine()
            };
        }

        public IEnumerable<IOcrEngine> Engines => _engines;

        public bool IsReady => _engines.Any(x => x.IsSupported && x.IsInstalled && x.InstalledLanguages.Any());

        public bool IsNewestReady
        {
            get
            {
                var latest = _engines.FirstOrDefault(x => x.IsSupported);
                if (latest == null) return false;
                return latest.IsInstalled && latest.InstalledLanguages.Any();
            }
        }

        public bool CanUpgrade => !IsNewestReady && _engines.Any(x => x.IsInstalled);

        public bool MustUpgrade => !IsReady && _engines.Any(x => x.IsInstalled);

        public bool MustInstallPackage => _engines.All(x => (!x.IsSupported || !x.CanInstall) && !x.IsInstalled);

        public IOcrEngine ActiveEngine => _engines.FirstOrDefault(x => x.IsSupported && x.IsInstalled && x.InstalledLanguages.Any());

        public IOcrEngine InstalledEngine => _engines.FirstOrDefault(x => x.IsInstalled && x.InstalledLanguages.Any());

        public IOcrEngine EngineToInstall => _engines.FirstOrDefault(x => x.IsSupported && x.CanInstall);
    }
}
