{{- define "fcg-game-api.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" -}}
{{- end }}

{{- define "fcg-game-api.fullname" -}}
fcg-user-api
{{- end }}

{{- define "fcg-user-api.fullname" -}}
fcg-user-api
{{- end }}

{{- define "fcg-game-api.labels" -}}
helm.sh/chart: {{ .Chart.Name }}-{{ .Chart.Version | replace "+" "_" }}
app.kubernetes.io/name: {{ include "fcg-game-api.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
app.kubernetes.io/version: {{ .Chart.AppVersion }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{- define "fcg-game-api.selectorLabels" -}}
app.kubernetes.io/name: {{ include "fcg-game-api.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{- define "fcg-game-api.serviceAccountName" -}}
{{- if .Values.serviceAccount.name -}}
{{- .Values.serviceAccount.name -}}
{{- else -}}
{{- include "fcg-game-api.fullname" . -}}
{{- end -}}
{{- end }}