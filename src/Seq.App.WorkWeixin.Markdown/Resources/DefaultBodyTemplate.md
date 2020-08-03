### {{$Level}}
{{$LocalTimestamp}}

------------

{{#if_eq $EventType "$A1E77000"}}
**Alert condition *{{Condition}}* detected on [{{DashboardTitle}}/{{ChartTitle}}]({{DashboardUrl}})**

[Explore detected results in Seq]({{ResultsUrl}})

> **Query**
	- [ ] `{{Query}}`
> Measurement window
  - [ ] {{MeasurementWindow}}
> Suppression time
  - [ ] {{SuppressionTime}}
> Detected range start
  - [ ] {{AlertRangeStart}}
> Detected range end
  - [ ] {{AlertRangeEnd}}
> Signal
{{#if SignalTitles}} 
  - [ ] {{pretty SignalTitles}}
{{else}} 
  - [ ] {{pretty SignalExpression}}
{{/if}}
{{#if OwnerUsername}}
> Owner
  - [ ] {{OwnerUsername}}
{{/if}}
{{#if Errors}}
> **Error**
{{#each Errors}}
  - [ ] {{this}}
{{/each}}
{{/if}}
{{#if Results}}
> **Results**
{{#each Results}}
  - [ ] {{pretty this.Key}}
{{#each Slices}}
  - [ ] **{{SliceStart}}** {{pretty Rowset}}
{{/each}}
{{/each}}
{{/if}}
{{else}}
**{{$Message}}**

[Open this event in Seq]({{$ServerUri}}#/events?filter=@Id%20%3D%20'{{$Id}}'&amp;show=expanded)

{{#each $Properties}}
> ** {{@key}} **
	*{{pretty this}}*

{{/each}}
{{#if $Exception}}
**Exception**
```
{{$Exception}}
```
{{/if}}
{{/if_eq}}

------------

Sent by Seq installed at <{{$ServerUri}}>.
