Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.ExpressApp.Scheduler.Win
Imports DevExpress.XtraScheduler

Namespace WinSolution.Module.Win
	Public Class SchedulerListViewViewController
		Inherits ViewController(Of ListView)
		Private listEditor As SchedulerListEditor
		Public Sub New()
			TargetObjectType = GetType(DevExpress.Persistent.BaseImpl.Event)
		End Sub
		Protected Overrides Overloads Sub OnViewControlsCreated()
			MyBase.OnViewControlsCreated()
			listEditor = TryCast(View.Editor, SchedulerListEditor)
			If listEditor IsNot Nothing Then
				Dim scheduler As SchedulerControl = CType(listEditor.SchedulerControl, SchedulerControl)
				RemoveHandler scheduler.InitAppointmentDisplayText, AddressOf scheduler_InitAppointmentDisplayText
				AddHandler scheduler.InitAppointmentDisplayText, AddressOf scheduler_InitAppointmentDisplayText
			End If
		End Sub
		Protected Overrides Overloads Sub OnDeactivated()
			MyBase.OnDeactivated()
			If listEditor IsNot Nothing AndAlso listEditor.SchedulerControl IsNot Nothing Then
				Dim scheduler As SchedulerControl = CType(listEditor.SchedulerControl, SchedulerControl)
				RemoveHandler scheduler.InitAppointmentDisplayText, AddressOf scheduler_InitAppointmentDisplayText
			End If
		End Sub
		Private Sub scheduler_InitAppointmentDisplayText(ByVal sender As Object, ByVal e As AppointmentDisplayTextEventArgs)
			Dim appointment As Appointment = e.Appointment
			If appointment.IsRecurring Then
				appointment = e.Appointment.RecurrencePattern
			End If
			Dim obj As DevExpress.Persistent.Base.General.IEvent = CType(listEditor.SourceObjectHelper.GetSourceObject(appointment), DevExpress.Persistent.Base.General.IEvent)
			e.Text = String.Format("{0}-({1})", e.Text, CType(obj, Object).GetHashCode())
		End Sub
	End Class
End Namespace