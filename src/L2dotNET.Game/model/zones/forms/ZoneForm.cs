
namespace L2dotNET.GameService.model.zones.forms
{
    public class ZoneForm
    {
        public virtual bool isInsideZone(int x, int y, int z) { return false; }

        public virtual bool intersectsRectangle(int x1, int x2, int y1, int y2) { return false;  }

        public virtual double getDistanceToZone(int x, int y) { return 0; }

        public virtual int getLowZ() { return 0; } //Support for the ability to extract the z coordinates of zones.

        public virtual int getHighZ() { return 0; } //New fishing patch makes use of that to get the Z for the hook

    }
}
