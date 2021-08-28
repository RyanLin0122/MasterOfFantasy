using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class PlayerStorage
{

    //private ReentrantReadWriteLock mutex = new ReentrantReadWriteLock();
    //private Lock rL = mutex.readLock(), wL = mutex.writeLock();
    //private ReentrantReadWriteLock mutex2 = new ReentrantReadWriteLock();
    // private Lock rL2 = mutex2.readLock(), wL2 = mutex2.writeLock();
    //private Dictionary<String, MapleCharacter> nameToChar = new Dictionary<string, MapleCharacter>();
    //private Dictionary<int, MapleCharacter> idToChar = new Dictionary<Integer, MapleCharacter>();
    //private Dictionary<int, CharacterTransfer> PendingCharacter = new Dictionary<int, CharacterTransfer>();
    private int channel;

    public PlayerStorage(int channel)
    {
        this.channel = channel;
        // Prune once every 15 minutes
        //PingTimer.getInstance().schedule(new PersistingTask(), 900000);
    }

}

