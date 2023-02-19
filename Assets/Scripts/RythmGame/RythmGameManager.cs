using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.IO;
using System.Runtime.Serialization;
public class RythmGameManager : MonoBehaviour
{
    [Header("Stats")]
    [Header("Energy")]
    public float energyMax = 2; 
    public float energyLossRate = 1;
    public float energyRegenRate = 5;
    [Space()] public float lockoutTime = 3f;
    [Space(), Header("Hit Range")]
    public float normalHitRange = 0.5f;
    public float perfectHitRange = 0.2f;
    [Header("Speed")]
    public float noteSpeed = 2;
    public float beatsPerMinute = 120;
    [Header("Note map")]
    [SerializeField] public List<Chord> map;
    
    [Header("References")]
    [SerializeField] private GameManager inputManager;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private AudioSource sfxSource;

    [Space(), Header("Rythmlanes")]
    public RythmLane Lane1;
    public RythmLane Lane2;
    public RythmLane Lane3;
    public RythmLane Lane4;

    [Header("Debug Info")]
    public int progress;
    public UnityEngine.Events.UnityEvent OnLaneComplete;
    [System.Serializable] public class RythmLane{
        public RythmGameManager manager;
        public int laneNotesCompleted;

        [SerializeField] private float curEnergy;        
        [SerializeField] private Transform noteSpawnPoint;
        [SerializeField] private Transform noteHitPoint;
        [SerializeField] private List<Beat> beats;

        private Coroutine lockoutHandler;
        public event System.Action normalHit;
        public event System.Action perfectHit;
        public event System.Action sustainedNote;
        public event System.Action miss;
        public bool isActivated, isLockedOut;
        [SerializeField] private UnityEngine.UI.Image renderer;
        [SerializeField] private Sprite DefaultState,ActivatedState,LockedOutState;



        public void addBeat(BeatInstruction b){
            if (b == BeatInstruction.None)
                return;
            Transform T = Instantiate(manager.notePrefab).transform;
            T.position = noteSpawnPoint.position;
            T.SetParent(noteSpawnPoint);
            beats.Add(new Beat(T, b));
        }
        public void removeBeat(Beat beat, HitType HT){
            switch (HT){
                case HitType.Normal : 
                    normalHit?.Invoke();
                break;
                case HitType.Perfect : 
                    perfectHit?.Invoke();
                break;
                case HitType.Sustained : 
                    sustainedNote?.Invoke();
                break;
                case HitType.Miss : 
                    miss?.Invoke();
                break;
            }
            laneNotesCompleted++;
            Destroy(beat.transform.gameObject);
            beats.Remove(beat);
        }

        private void MovementTick(){
            if(beats.Count == 0)
                return;

            //Movement logic for the beats
            if(!isActivated || isLockedOut)
                curEnergy += manager.energyRegenRate * Time.deltaTime;
            if(curEnergy > manager.energyMax)
                curEnergy = manager.energyMax;
            Vector3 move = new Vector3(0, manager.noteSpeed * Time.deltaTime, 0);
            foreach (var Beat in beats)
                Beat.transform.position -= move;
            for (int i = 0; i < beats.Count; i++){
                if(beats[i].transform.position.y < noteHitPoint.transform.position.y - manager.normalHitRange){
                    removeBeat(beats[i], HitType.Miss);
                    i-=1;
                }
            }
        }
        private void VisualTick(){
            if(isActivated)
                renderer.sprite = ActivatedState;
            else 
                renderer.sprite = DefaultState;
            if(isLockedOut)
                renderer.sprite = LockedOutState;
        }
        private void InputTick(){
            if(beats.Count == 0)
                return;
            if (!isActivated || isLockedOut)
                return;
            Beat nextBeat = beats[0];


            if (nextBeat.type == BeatInstruction.NormalBeat)
            { //handle a regular beat incoming
                if (Vector3.Distance(nextBeat.transform.position, noteHitPoint.position) <= manager.perfectHitRange)
                {
                    removeBeat(nextBeat, HitType.Perfect);
                    curEnergy = manager.energyMax;
                }
                else if (Vector3.Distance(nextBeat.transform.position, noteHitPoint.position) <= manager.normalHitRange)
                {
                    removeBeat(nextBeat, HitType.Normal);
                    curEnergy += manager.energyMax * 0.25f;
                }
            }
            else if (nextBeat.type == BeatInstruction.Sustained)
            { //handle a sustained beat incoming
                curEnergy += manager.energyLossRate * 1.25f * Time.deltaTime;
                if (Vector3.Distance(nextBeat.transform.position, noteHitPoint.position) <= manager.perfectHitRange)
                    removeBeat(nextBeat, HitType.Sustained);
            }

            curEnergy -= manager.energyLossRate * Time.deltaTime;
            if (curEnergy <= 0)
                startLockoutLogic();
        }
        public void Tick(){
            MovementTick();
            InputTick();
            VisualTick();
        }
        public void startLockoutLogic(){
            if (lockoutHandler != null)
                return;
            lockoutHandler = manager.StartCoroutine(lockoutLogic());
        }
        private IEnumerator lockoutLogic()
        {
            float remainingTime = manager.lockoutTime;
            isLockedOut = true;
            do
            {
                remainingTime -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            } while (remainingTime > 0);
            isLockedOut = false;
            lockoutHandler = null;
        }
    }

    private void OnDisable() {
        inputManager.inputActions.RythmGame.Disable();
    }
    private void Start() {
        inputManager = GameManager.Instance;
        Lane1.manager = this;
        Lane2.manager = this;
        Lane3.manager = this;
        Lane4.manager = this;
        
        #region Laneinput setup
        inputManager.inputActions.RythmGame.ActivateLane1.performed += _ => {
            Lane1.isActivated = true;
            sfxSource.Play();
        };
        inputManager.inputActions.RythmGame.ActivateLane1.canceled += _ => Lane1.isActivated = false;

        inputManager.inputActions.RythmGame.ActivateLane2.performed += _ => {
            Lane2.isActivated = true;
            sfxSource.Play();
        };
        inputManager.inputActions.RythmGame.ActivateLane2.canceled += _ => Lane2.isActivated = false;

        inputManager.inputActions.RythmGame.ActivateLane3.performed += _ => {
            Lane3.isActivated = true;
            sfxSource.Play();
        };
        inputManager.inputActions.RythmGame.ActivateLane3.canceled += _ => Lane3.isActivated = false;

        inputManager.inputActions.RythmGame.ActivateLane4.performed += _ => {
            Lane4.isActivated = true;
            sfxSource.Play();
        };
        inputManager.inputActions.RythmGame.ActivateLane4.canceled += _ => Lane4.isActivated = false;
        #endregion
    
        inputManager.inputActions.RythmGame.Enable();
        StartCoroutine(spawnNotes());
    }
    private void Update() {
        Lane1.Tick();
        Lane2.Tick();
        Lane3.Tick();
        Lane4.Tick();

        if(progress == map.Count)
            OnLaneComplete?.Invoke();
    }
    private int notesLeft(){
        int lane1count = map.Count - Lane1.laneNotesCompleted; 
        int lane2count = map.Count - Lane2.laneNotesCompleted; 
        int lane3count = map.Count - Lane3.laneNotesCompleted; 
        int lane4count = map.Count - Lane4.laneNotesCompleted; 

        return lane1count + lane2count + lane3count + lane4count;
    }

    private IEnumerator spawnNotes(){
        progress = 0;
        while (progress < map.Count){
            Lane1.addBeat(map[progress].Lane1Instruction);
            Lane2.addBeat(map[progress].Lane2Instruction);
            Lane3.addBeat(map[progress].Lane3Instruction);
            Lane4.addBeat(map[progress].Lane4Instruction);
            progress++;
            yield return new WaitForSeconds(60f / beatsPerMinute);
        }
    }


    #region Save/Load map
    [Header("Saving map")]
    [SerializeField] private static string filePath = "Maps";
    [SerializeField] private string fileName = "new Map";
    private string path => $"{Application.dataPath}/{filePath}/{fileName}.xml";
    [System.Serializable] public struct mapSave{
        public mapSave(float _BPM, float _Speed, List<Chord> _Map){
            BPM = _BPM;
            Speed = _Speed;
            Map = _Map;
        }
        public float BPM;
        public float Speed;
        public List<Chord> Map;
    }

    [ContextMenu("Save map")]
    public void SaveCurrentMapInstructions(){
        DataContractSerializer serializer = new DataContractSerializer(typeof(mapSave));

        using(FileStream fileStream = File.Open(path, FileMode.Create)){
            serializer.WriteObject(fileStream, new mapSave(beatsPerMinute, noteSpeed, map));
        }
    }

    [ContextMenu("Load Map")]
    public void LoadMapInstructions(){
        DataContractSerializer reader = new DataContractSerializer(typeof(mapSave));

        using(FileStream fileStream = File.OpenRead(path)){
            mapSave t = (mapSave)reader.ReadObject(fileStream);

            noteSpeed = t.Speed;
            beatsPerMinute = t.BPM;
            map = t.Map;
        }
    }
    #endregion
}
