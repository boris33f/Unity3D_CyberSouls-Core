using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using FMODUnity;

public class MusicManager : MonoBehaviour
{
    //Event System
    public delegate void OnBeat();
    public static event OnBeat OnBeatDetect;

    //Small Logic
    private int lastBeat;
    private int thisBeat;

    //Music Manager Logic
    public static MusicManager me;
    [SerializeField] [EventRef] private string music; //The menu to choose an event

    //TimelineInfo Class
    [StructLayout(LayoutKind.Sequential)] public class TimelineInfo
    {
        public int currentBeat = 0;
        public int currentBar = 0;
        public float currentTempo = 0;
        public int currentPosition = 0;
        public float songLenght = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    public TimelineInfo timelineInfo = null; //creating a variable of type TimelineInfo (class)

    private GCHandle timelineHandle;
    private FMOD.Studio.EVENT_CALLBACK beatCallback; //to get info
    private FMOD.Studio.EventDescription descriptionCallback; //to get duration

    public FMOD.Studio.EventInstance musicPlayEvent; //an instance of the event set above

    private void Awake()
    {
        //self-referencing
        me = this;    

        //music event instance to handle
        musicPlayEvent = RuntimeManager.CreateInstance(music);
        musicPlayEvent.start();
    }

    private void Start()
    {
        timelineInfo = new TimelineInfo();
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);

        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        musicPlayEvent.setUserData(GCHandle.ToIntPtr(timelineHandle));
        musicPlayEvent.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);

        musicPlayEvent.getDescription(out descriptionCallback);
        descriptionCallback.getLength(out int length);

        timelineInfo.songLenght = length;
    }

    private void Update()
    {
        musicPlayEvent.getTimelinePosition(out timelineInfo.currentPosition);
    }

    //BeatEventCallback Function
    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero) //System(IntPtr)
        {
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                {
                    var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                    timelineInfo.currentBeat = parameter.beat;
                    timelineInfo.currentBar = parameter.bar;
                    timelineInfo.currentTempo = parameter.tempo;
                }
                break;

                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                {
                    var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                    timelineInfo.lastMarker = parameter.name;
                }
                break;
            }
        }
        return FMOD.RESULT.OK;
    }

    private void FixedUpdate()
    {
        lastBeat = timelineInfo.currentBeat;
        if (lastBeat != thisBeat)
        {
            thisBeat = lastBeat;
            OnBeatDetect();
        }
    }

    private void OnDestroy()
    {
        musicPlayEvent.setUserData(IntPtr.Zero);
        musicPlayEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicPlayEvent.release();
        timelineHandle.Free();
    }
}
