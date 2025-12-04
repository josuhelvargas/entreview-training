pipeline {
  agent any

  tools {
    jdk 'jdk17'
    maven 'maven3'
  }

  stages {

    stage('Checkout') {
      steps { checkout scm }
    }

    stage('Build') {
      steps { sh 'mvn -B clean verify -Pqa' }
    }

    stage('Package') {
      steps { sh 'mvn -B package -Prelease' }
    }

    stage('Archive Artifact') {
      steps { archiveArtifacts artifacts: 'target/*.jar', fingerprint: true }
    }

    stage('Deploy to Dev') {
      steps { sh 'scp target/myapp.jar user@server:/apps' }
    }
  }
}
