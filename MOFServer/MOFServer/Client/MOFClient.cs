using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Core.Session;

class MOFClient
{
    //private static long serialVersionUID = 9179541993413738569L;

    public static int DEFAULT_CHARSLOT = 3;
    public static string CLIENT_KEY = "CLIENT";
    //private MOFAESOFB send, receive;
    private IoSession session;
    private MOFCharacter player;
    private int channel = 1, accId = 1, world;
    private int charslots = DEFAULT_CHARSLOT;
    private string accountName;
    //private long lastPong = 0, lastPing = 0;
    private bool monitored = false, receiving = true;
    public short loginAttempt = 0;
    private List<int> allowedChar = new List<int>();
    //private Set<String> macs = new HashSet<String>();
    //private Dictionary<string, ScriptEngine> engines = new Dictionary<string, ScriptEngine>();
    //private ScheduledFuture<?> idleTask = null;
    private string secondPassword; // To be used only on login


    public MOFClient(IoSession session)
    {
        this.session = session;
    }
    public IoSession getSession()
    {
        return session;
    }

    public MOFCharacter getPlayer()
    {
        return player;
    }

    public void setPlayer(MOFCharacter player)
    {
        this.player = player;
    }

    public void createdChar(int id)
    {
        allowedChar.Add(id);
    }
    /*
    public List<MOFCharacter> loadCharacters(int accountmap)
    { // TODO make this less costly 
        List<MOFCharacter> chars = new List<MOFCharacter>();

        foreach (CharNameAndId cni in loadCharactersInternal(serverId))
        {
            MOFCharacter chr = MOFCharacter.loadCharFromDB(cni.id, this, false);
            chars.Add(chr);
            allowedChar.Add(chr.getId());
        }
        return chars;
    }
    */
    /*
    public class CharNameAndId
    {
        public string name;
        public int id;
        public CharNameAndId(string name, int id)
        {
            this.name = name;
            this.id = id;
        }
    }
    */
    
}


