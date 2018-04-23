﻿using System;
using log4net;
using L2dotNET.Services.Contracts;

namespace L2dotNET.LoginService
{
    public class PreReqValidation
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PreReqValidation));

        private readonly ICheckService _checkService;

        public PreReqValidation(ICheckService checkService)
        {
            _checkService = checkService;
        }

        public void Initialize()
        {
            if (_checkService.PreCheckRepository())
                return;

            Log.Warn("Some checks have failed. Please correct the errors and try again.");
            Log.Info("Press ENTER to exit...");
            Console.Read();
            Environment.Exit(0);
        }
    }
}