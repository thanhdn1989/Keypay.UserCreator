BUG
   
As far as I can see, the Id duplication issue can be because:
1. Threading issue
+ Root cause

    StreamWriter is not thread safe. Taking a look here: [streamwriter.cs
  ](https://github.com/microsoft/referencesource/blob/master/mscorlib/system/io/streamwriter.cs), it use a global buffer char array, so when multiple thread trying to invoke WriteAsync method on the same StreamWriter instance, one thread's line can overwrite the previous thread hence cause duplicate row => duplicate Id
  + Solution:

      Use a thread synchronization mechanism like: Mutex, Semaphore etc. In this case I will use SemaphoreSlim since it can be released on multi-thread

  Update:

  + Make DoWriteAsync include (Generate Id, Generate csv row, write/flush to file) as an atomic operation => Ensure that there will be no threading issuse
  + Trim input when generate csv line so that prevent the hacky case user try to input DateOfBirth_{space} hence it cause duplicate ID with exist one

2. User use the same file after they quit or application crash
+ Root cause

    There is no mechanism for recovery yet, hence when user use the same file for their second run the ID will be generated from 1 again'
+ Solution

    Always check if file is exist, if exists then we will try to find the latest ID of each field and start from there

ENHANCEMENT

   To prevent user's document lost when their computer crash, I mimic the behavior of MS Office that will perform an autosave action every 5 minute.
   When user recover back to their session, we will try to find latest ID of each Field and then continue from there