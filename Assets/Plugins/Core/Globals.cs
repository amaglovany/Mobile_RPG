using TDC.Profile;
using TDC.SaveAndLoad;

namespace TDC
{
    public class Globals : Singleton<Globals>
    {
        //Local
        public CoreProfile coreProfile;
        public CoreWorldTime coreWorldTime;
        public CorePlayerPrefs corePlayerPrefs;

#if GoogleMobileAds
        public CoreAdMob coreAdMob;
        #endif

        //Public

        public virtual void Initialization()
        {
            coreProfile = new CoreProfile();
            coreWorldTime = new CoreWorldTime();
            corePlayerPrefs = new CorePlayerPrefs();
        }

        public virtual void CoreUpdate()
        {
            coreWorldTime.CoreUpdate();
        }
    }
}