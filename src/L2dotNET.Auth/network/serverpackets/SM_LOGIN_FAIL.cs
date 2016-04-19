using System;

namespace L2dotNET.Auth.serverpackets
{
    public class SM_LOGIN_FAIL : SendBasePacket
    {
        public enum LoginFailReason
        {
            NOTHING = 0,
            SYSTEM_ERROR_TRY_AGAIN = 1,
            PASS_WRONG = 2,
            USER_OR_PASS_WRONG = 3,
            ACCESS_FAILED_TRY_AGAIN = 4,
            INCORRECT_ACCOUNT_INFO = 5,
            ACCOUNT_IN_USE = 7,
            TOO_YOUNG = 12,
            SERVER_OVERLOADED = 15,
            SERVER_MAINTENANCE = 16,
            CHANGE_TEMP_PASS = 17,
            TEMP_PASS_EXPIRED = 18,
            NO_TIME_LEFT = 19,
            SYSTEM_ERROR = 20,
            ACCESS_FAILED = 21,
            RESTRICTED_IP = 22,
            SECURITY_CARD = 31,
            NOT_VERIFY_AGE = 32,
            NO_ACCESS_COUPON = 33,
            DUAL_BOX = 35,
            INACTIVE_REACTIVATE = 36,
            ACCEPT_USER_AGREEMENT = 37,
            GUARDIAN_CONSENT = 38,
            DECLINED_AGREEMENT_OR_WIDTHDRAWL = 39,
            ACCOUNT_SUSPENDED = 40,
            CHANGE_PASS_QUIZ = 41,
            ACCESSED_10_ACCOUNTS = 42
        }
        /*
            1 System error, please log in again later.
            2 Password does not match this account. Confirm your account information and log in again later.
            4 Access failed. Please try again later.
            5 Your account information is incorrect. For more details, please contact our Support Center at http://support.plaync.com
            7 This account is already in use.  Access denied.
            12 Lineage II game services may be used by individuals 15 years of age or older except for PvP servers, which may only be used by adults 18 years of age and older. (Korea Only)
            15 Due to high server traffic, your login attempt has failed.  Please try again soon.
            16 Server under maintenance. Please try again later.
            17 Please login after changing your temporary password.
            18 Your usage term has expired. PlayNC website (http://www.plaync.com/us/support/)
            19 There is no time left on this account.
            20 System error.
            21 Access failed.
            22 This server is reserved for players in Korea.  To play Lineage II, please connect to the server in your region.
            23 -
            30 This week's usage time has finished.
            31 The security card number is invalid.
            32 Users who have not verified their age may not log in between the hours of 10:00 p.m. and 6:00 a.m.
            33 This server cannot be accessed by the coupon you are using.
            35 You are using a computer that does not allow you to log in with two accounts at the same time.
            36 Your account is currently inactive because you have not logged into the game for some time. You may reactivate your account by visiting the PlayNC website (http://www.plaync.com/us/support/).
            37 You must accept the User Agreement before this account can access Lineage II.\n Please try again after accepting the agreement on the PlayNC website (http://www.plaync.com/us/support/)
            38 A guardian's consent is required before this account may be used to play Lineage II.\nPlease try again after obtaining this consent.
            39 This account has declined the User Agreement or a withdrawal request is pending for it.\nPlease try again after canceling this request.
            40 This account was converted into an integrated account and as a result may not access Lineage II.\nPlease try again with the integrated account.
            41 Your account may only be used after changing your password and quiz. \n To make the necessary changes, please visit the PlayNC website (http://www.plaync.com/us/support/).
            42 You are currently logged into 10 of your accounts and can no longer access your other accounts.
         * */

        LoginFailReason _reason;
        public SM_LOGIN_FAIL(LoginClient Client, LoginFailReason reason)
        {
            base.makeme(Client);
            _reason = reason;
        }

        protected internal override void write()
        {
            writeC(0x01);
            writeC((byte)_reason);
        }
    }
}
