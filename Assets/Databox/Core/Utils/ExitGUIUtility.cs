﻿ using System;
 using System.Reflection;
 using UnityEngine;
 
 namespace Databox.Utils
 {
	 public static class ExitGUIUtility
	 {
		 public static bool ShouldRethrowException(Exception exception)
		 {
			 return IsExitGUIException(exception);
		 }
	 
		 public static bool IsExitGUIException(Exception exception)
		 {
			 while (exception is TargetInvocationException && exception.InnerException != null)
			 {
				 exception = exception.InnerException;
			 }
			 return exception is ExitGUIException;
		 }
	 }
 }