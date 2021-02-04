using System;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
//using System.Messaging; 

namespace ExceptionHandling
{
    class Program
    {
        static void Main(string[] args)
        {
            //Guide user
            Console.WriteLine("Please enter 1 to run Catching Multiple Exceptions method");
            Console.WriteLine("Please enter 2 to Exception Properties method");
            Console.WriteLine("Please enter 3 to Dispatch Info method");

            try {
                switch(Console.ReadLine()){
                    //if user entered 1
                    case "1": {
                        //Run method
                        CatchingMultipleExceptions();
                        break;
                    }
                    //If user entered 2
                    case "2": {
                        //Run method
                        ExceptionProperties();
                        break;
                    }
                    //If user entered 3
                    case "3": {
                        //Run method
                        DispatchInfo();
                        break;
                    }
                    //If user entered something that doesn't correspond with the provided options
                    default: {
                        //show message
                        Console.WriteLine("Your entered number does not correspond with any of the provided options.");
                        break;
                    }
                }
            }
            catch(Exception e) {
                //Catch any exceptions that could occur and handle them
                Console.WriteLine("Exception caught: {0}", e.Message);
            }
            finally {
                //Goodbye message
                Console.WriteLine("Thank you!");
            }
        }
        
        //Catch multipe exceptions method
        public static void CatchingMultipleExceptions()
        {
            //Instruct user
            Console.WriteLine("Please enter a number");

            //Get user input
            string s = Console.ReadLine();

            try
            {
                //try to cast it as int
                int i = int.Parse(s);

                //To prevent finally block from running, uncomment this statement: 
                //Enviroment.FailFast("Error");
            }
            //catch argument null exception if user hasn't entered a value
            catch(ArgumentNullException)
            {
                //Show message
                Console.WriteLine("You need to enter a value");
            }
            //Catch forbidden casting exception
            catch(FormatException){
                //Show message
                Console.WriteLine("{0} is not a valid number. Please try again.", s);
            }
            //Finally block
            finally {
                Console.WriteLine("Program complete.");
            }
        }

        //Exception properties method
        //
        //Working with Exception class properties
        public static void ExceptionProperties()
        {
            //Guide user
            Console.WriteLine("Please enter a number");

            try
            {
                //Convert string to int
                int i = int.Parse(Console.ReadLine());
                Console.WriteLine("Parsed: {0}", i);
            }
            //Catch casting exception
            catch (FormatException e)
            {
                //Human friendly message that dexcribes exception
                Console.WriteLine("Message: {0}", e.Message);

                //string that decribes all running methods that are currently executing
                Console.WriteLine("StackTrace: {0}", e.StackTrace);

                //URL That points to help file
                Console.WriteLine("HelpLink: {0}", e.HelpLink);

                //Two exceptions are linked together with inner exception if another one happened
                Console.WriteLine("InnerException: {0}", e.InnerException);

                //Contains name of method that caused exception to occur
                Console.WriteLine("TargetSite: {0}", e.TargetSite);

                //Name of the Application that caused the eexception
                Console.WriteLine("Source: {0}", e.Source);

                //Rethrows orignal exception + Holds original strack trace
                throw;
            }
        }

        //Message-QueueException method
        //
        //It's used to help external parties understand inner exceptions that are thrown(Other users don't understand your module or internal workings of your app)
        public static void MessageQueueException(){
            //Unfortunately, i can't find the System.Messaging namespace inside the system namespace, because it says that it doesn't exist in .net core 5.0
            // try {
            //     double o = 2.2;
            //     int i = (int)o;
            // }
            // catch(MessageQueueException e){
            //     throw new OrderProcessingException("Error while processing order", e);
            // }
        }

        //DispatchInfo method
        //
        //The ExceptionDispatchInfo class is used to throw exceptions and preserve their original stack trace
        public static void DispatchInfo(){
            //Local variable
            ExceptionDispatchInfo possibleException = null;

            try {
                //Get user input
                string s = Console.ReadLine();
                //Possible format exception
                int.Parse(s);
            }
            catch(FormatException e){
                //Catch exception and save the stack trace to possibleException variable
                possibleException = ExceptionDispatchInfo.Capture(e);
            }
            if(possibleException != null){
                //Throw all caught exceptions that's saved in possibleExeption variable
                possibleException.Throw();
            }
        }

        //Custom Exception
        //Custom exeptions should always inherit from System.Exception namespace
        //Always need to add constructor(string, exception, serialization params)
        [Serializable]
        public class OrderProcessingException : Exception, ISerializable
        {

            //Constructor with ID
            public OrderProcessingException(int orderId)
            {
                OrderId = orderId;
                this.HelpLink = "http://www.mydomain.com/infoaboutexception";
            }
            //Constructor with ID and Message
            public OrderProcessingException(int orderId, string message) : base(message)
            {
                OrderId = orderId;
                this.HelpLink = "http://www.mydomain.com/infoaboutexception";
            }

            //Constructor with ID, Message and Exception
            public OrderProcessingException(int orderId, string message, Exception innerException): base(message, innerException)
            {
                OrderId = orderId;
                this.HelpLink = "http://www.mydomain.com/infoaboutexception";
            }
            
            protected OrderProcessingException(SerializationInfo info, StreamingContext context)
            {
                OrderId = (int)info.GetValue("OrderId", typeof(int));
            }

            public int OrderId { get; private set; }

            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("OrderId", OrderId, typeof(int));
            }
        }






    }
}
