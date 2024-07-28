﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SAS.StateMachineGraph.Editor
{
    [ExecuteInEditMode]
    internal class StateMachineEditorWindow : GridEditorWindow
    {
        protected RuntimeStateMachineController _runtimeStateMachineController;
        private StateMachineParameterEditor _parameterEditor = new StateMachineParameterEditor();
        private StateTransitionEditor _transition;

        private List<BaseNode> _nodes = new List<BaseNode>();
        private Actor Actor => Selection.activeGameObject?.GetComponent<Actor>();

        private List<StateMachineModel> _selectedChildStateMachines = new List<StateMachineModel>();
        private StateMachineModel SelectedStateMachineModel
        {
            get
            {
                if (_selectedChildStateMachines.Count > 0)
                    return _selectedChildStateMachines[_selectedChildStateMachines.Count - 1];
                return null;
            }
        }

        public static void ShowBehaviourGraphEditor(RuntimeStateMachineController target)
        {
            var detailsWindow = GetWindow<StateMachineEditorWindow>(typeof(SceneView));
            GUIContent content = new GUIContent("StateMachine");
            detailsWindow.titleContent = content;
            detailsWindow.ShowTab();
            detailsWindow.Repaint();
            Selection.activeObject = target;
            EditorPrefs.SetString("StateMachine", AssetDatabase.GetAssetPath(target));
        }

        public void Awake()
        {
            OnSelectionChange();
        }

        private void OnFocus()
        {
            OnSelectionChange();
        }

        void OnSelectionChange()
        {
            if (Selection.activeObject == null || Selection.activeObject is StateMachineOverrideController)
                return;

            var stateMachineModel = Selection.activeObject as RuntimeStateMachineController;
            if (stateMachineModel == null)
            {
                if (Actor != null)
                {
                    var actorSO = new SerializedObject(Actor);
                    stateMachineModel = actorSO.FindProperty("m_Controller").objectReferenceValue as RuntimeStateMachineController;
                    if (stateMachineModel is StateMachineOverrideController)
                        stateMachineModel = (stateMachineModel as StateMachineOverrideController).runtimeStateMachineController;
                }
            }
            else
                EditorPrefs.SetString("StateMachine", AssetDatabase.GetAssetPath(stateMachineModel));

            if (stateMachineModel != null)
            {
                _runtimeStateMachineController = stateMachineModel;
                if (_runtimeStateMachineController == null)
                {
                    if (EditorPrefs.HasKey("StateMachine"))
                        _runtimeStateMachineController = AssetDatabase.LoadAssetAtPath(EditorPrefs.GetString("StateMachine"), typeof(RuntimeStateMachineController)) as RuntimeStateMachineController;
                    if (_runtimeStateMachineController == null)
                        return;
                }

                Initialize();
            }
        }

        protected void OnEnable()
        {
            OnSelectionChange();
        }

        private void Initialize()
        {
            _selectedChildStateMachines.Clear();
            _selectedChildStateMachines.Add(_runtimeStateMachineController.BaseStateMachineModel());

            _parameterEditor = new StateMachineParameterEditor(_runtimeStateMachineController);
            _transition = new StateTransitionEditor(_runtimeStateMachineController);

            CreateSelectedStateMachineNodes();

        }

        private void CreateSelectedStateMachineNodes()
        {
            _transition.Clear();
            _nodes.Clear();

            var anyStateNode = new AnyStateNode(SelectedStateMachineModel, _runtimeStateMachineController.AnyStateModel(), SelectedStateMachineModel.GetAnyStatePosition(), StartTranstionFromAnyState);
            _nodes.Add(anyStateNode);

            var stateMachineModels = SelectedStateMachineModel.GetChildStateMachines();
            for (int i = 0; i < stateMachineModels.Count; ++i)
                CreateChildMachinelNode(stateMachineModels[i]);

            var stateModels = SelectedStateMachineModel.GetStates();
            for (int i = 0; i < stateModels.Count; ++i)
                CreateStateModelNode(stateModels[i]);

            var parentStateModel = SelectedStateMachineModel.GetParent();
            if (parentStateModel != null)
                CreateParentMachinelNode(parentStateModel);

            CreateTransitions();
            Repaint();
        }

        private void CreateTransitions()
        {
            foreach (BaseNode sourceNode in _nodes)
            {
                StateModel sourceStateModel = null;
                if (sourceNode is StateNode stateNode)
                {
                    sourceStateModel = stateNode.Value;
                    CreateTransitions(sourceNode, sourceStateModel);
                }
                else if (sourceNode is AnyStateNode anyStateNode)
                {
                    sourceStateModel = anyStateNode.Value;
                    CreateTransitions(sourceNode, sourceStateModel);
                }
                else if (sourceNode is StateMachineNode stateMachineNode)
                {
                    var stateModels = stateMachineNode.Value.GetStates();
                    foreach (var stateModel in stateModels)
                        CreateTransitions(sourceNode, stateModel);
                }
            }
        }

        private void CreateTransitions(BaseNode sourceNode, StateModel sourceStateModel)
        {
            if (sourceStateModel == null)
                return;

            var stateTransitions = sourceStateModel?.GetTransitionsProp();
            for (int i = 0; i < stateTransitions.arraySize; ++i)
            {
                var element = (StateTransitionModel)stateTransitions.GetArrayElementAtIndex(i).objectReferenceValue;
                var targetStateModel = element.serializedObject().FindProperty("m_TargetState").objectReferenceValue as StateModel;
                var targetNode = _nodes.Find(ele => ele.TargetObject == targetStateModel);
                if (targetNode == null)
                {
                    //check if target state belongs to any childStateMachine
                    if (sourceNode is AnyStateNode)
                        continue;
                    foreach (BaseNode node in _nodes)
                    {
                        if (node is StateMachineNode stateMachineNode)
                        {
                            if (stateMachineNode.Value.Contains(targetStateModel))
                            {
                                targetNode = node;
                                break;
                            }
                        }
                    }
                }
                _transition.Add(sourceNode, targetNode, sourceStateModel, targetStateModel);
            }
        }

        protected override void OnGUI()
        {
            var rect = position;
            rect.x -= position.x;
            rect.y -= position.y;
            rect.height -= 2;

            EditorZoomArea.Begin(Zoom, rect);
            base.OnGUI();
            DrawStateMachineWindow();
            EditorZoomArea.End();

            DrawToolBar();
            DrawParameterWindow();
            EditorUtilities.VerticalLine(new Rect(Mathf.Max(200, position.width / 5) - 2, 1, position.width, position.height), 2, Color.black);
        }

        void OnInspectorUpdate()
        {
            this.Repaint();
        }

        private void DrawStateMachineWindow()
        {
            if (_runtimeStateMachineController == null)
                return;

            SetCurrentActiveNode();

            _transition?.DrawConnectionLine(Event.current);
            _transition?.DrawConnections();
            DrawNodes();
            ProcessNodeEvents(Event.current);
            _transition?.ProcessConnectionEvents(Event.current);
            ProcessMouseEvent(Event.current);
        }

        private void DrawToolBar()
        {
            EditorUtilities.HorizontalLine(new Rect(0, -2, position.width, 21), 22, new Color(0.2196079f, 0.2196079f, 0.2196079f));
            DrawStateMachineToolBar(new Rect(0, -1, position.width, 20));
            EditorUtilities.HorizontalLine(new Rect(0, 20, position.width, 21), 1, Color.black);
        }

        private void DrawParameterWindow()
        {
            BeginWindows();

            if (Application.isPlaying && _runtimeStateMachineController != null)
                _parameterEditor = new StateMachineParameterEditor(_runtimeStateMachineController);

            var windowRect = GUI.Window(1, new Rect(0, -2, Mathf.Max(200, position.width / 5), position.height - 2), _parameterEditor.DrawParametersWindow, "", new GUIStyle());
            _parameterEditor.DrawRect(windowRect);
            EndWindows();
        }

        int selectedIndex = 0;
        private void DrawStateMachineToolBar(Rect rect)
        {
            if (SelectedStateMachineModel == null)
                return;

            rect.x = Mathf.Max(200, position.width / 5) - 10;
            var childStateMachines = _selectedChildStateMachines.Select(ele => ele.name).ToArray();

            selectedIndex = GUI.Toolbar(rect, _selectedChildStateMachines.Count - 1, childStateMachines, Settings.ChildStateMachinestoolBarStyle);

            if (selectedIndex != _selectedChildStateMachines.Count - 1)
            {
                _selectedChildStateMachines.RemoveRange(selectedIndex + 1, (_selectedChildStateMachines.Count - selectedIndex) - 1);
                CreateSelectedStateMachineNodes();
            }
        }

        protected override void ProcessMouseEvent(Event e)
        {
            base.ProcessMouseEvent(e);
            switch (e.type)
            {
                case EventType.MouseUp:
                    if (e.button == 1)
                        ProcessContextMenu(e.mousePosition);

                    else if (e.button == 0)
                        _transition.ClearConnectionSelection();
                    break;
            }
        }

        private void ProcessNodeEvents(Event e)
        {
            if (_nodes != null)
            {
                for (int i = _nodes.Count - 1; i >= 0 && i < _nodes.Count; i--)
                {
                    if (_nodes[i].ProcessEvents(e))
                    {
                        Selection.activeObject = _nodes[i].TargetObject;
                        GUI.changed = true;
                        e.Use();
                    }
                }
            }
        }

        private void DrawNodes()
        {
            foreach (BaseNode node in _nodes)
                node.Draw();
        }

        protected override void OnDrag(Vector2 delta)
        {
            base.OnDrag(delta);
            if (_nodes != null)
            {
                for (int i = 0; i < _nodes.Count; i++)
                    _nodes[i].Drag(delta.ToVector2Int());
            }
            GUI.changed = true;
        }

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Create State"), false, () => AddState(mousePosition));
            genericMenu.ShowAsContext();

            genericMenu.AddItem(new GUIContent("Create Sub-State Machine"), false, () => AddChildStateMachine(mousePosition));
            genericMenu.ShowAsContext();

            genericMenu.AddItem(new GUIContent("Duplicate current StateMachine"), false, () => DuplicateCurrentStateMachine(mousePosition));
            genericMenu.ShowAsContext();
        }

        private void AddState(Vector2 mousePosition)
        {
            var stateModel = _runtimeStateMachineController.AddState(SelectedStateMachineModel, "New State", mousePosition.ToVector3Int());
            CreateStateModelNode(stateModel);
        }

        private void AddChildStateMachine(Vector2 mousePosition)
        {
            var stateMachineModel = _runtimeStateMachineController.AddChildStateMachine(SelectedStateMachineModel, "New StateMachine", mousePosition);
            CreateChildMachinelNode(stateMachineModel);
        }

        private void DuplicateCurrentStateMachine(Vector2 mousePosition)
        {
            CreateChildMachinelNode(SelectedStateMachineModel.CloneMachineRecursivily(_runtimeStateMachineController, SelectedStateMachineModel, mousePosition));
        }

        private void DuplicateStateMachine(StateMachineNode stateMachineNode)
        {
            CreateChildMachinelNode(stateMachineNode.Value.CloneMachineRecursivily(_runtimeStateMachineController, SelectedStateMachineModel));
        }

        private void CreateStateModelNode(StateModel stateModel)
        {
            StateNode node;
            bool isDefaultState = (stateModel == _runtimeStateMachineController.GetDefaultState() || _runtimeStateMachineController.GetAllStateModels().Count == 1);
            Action<StateNode> action = RemoveStateModelNode;
            if (isDefaultState)
                action = RemoveDefaultStateModelNode;
            node = new StateNode(stateModel, (Vector2Int)stateModel.GetPosition(), isDefaultState, StartTranstion, MakeTranstion, action, SetAsDefaultNode, DuplicateNode);
            if (isDefaultState)
                SetAsDefaultNode(node, false);

            _nodes.Add(node);
        }

        private void CreateChildMachinelNode(StateMachineModel stateMachineModel)
        {
            var node = new StateMachineNode(stateMachineModel, (Vector2Int)stateMachineModel.GetPosition(), _runtimeStateMachineController.IsDefaultStateMachine(stateMachineModel), MakeTranstion, StateMachineModelMouseUp, RemoveStateMachineNode, SelectStateMachineNode, DuplicateStateMachine);
            _nodes.Add(node);
            Repaint();
        }

        private void CreateParentMachinelNode(StateMachineModel stateMachineModel)
        {
            var node = new ParentStateMachineNode(stateMachineModel, (Vector2Int)stateMachineModel.GetPositionAsUpNode(), _runtimeStateMachineController.IsDefaultStateMachine(stateMachineModel), MakeTranstion, StateMachineModelMouseUp, GoToMachineNode);
            _nodes.Add(node);
        }

        private void StateMachineModelMouseUp(StateMachineNode stateMachineNode)
        {
            if (_transition.SourceStateModel == null)
                return;
            if (!stateMachineNode.CreateAvailableStatesGenericMenu())
                _transition.ClearConnectionSelection();
        }


        private void StartTranstionFromAnyState(BaseNode sourceNode)
        {
            var stateNode = sourceNode as AnyStateNode;
            _transition.Start(stateNode, stateNode.Value);
        }

        private void StartTranstion(BaseNode sourceNode)
        {
            var stateNode = sourceNode as StateNode;
            _transition.Start(stateNode, stateNode.Value);
        }

        private void MakeTranstion(BaseNode targetNode)
        {
            var targetStateNode = targetNode as StateNode;
            MakeTranstion(targetNode, targetStateNode.Value);
            Repaint();
        }

        private void MakeTranstion(BaseNode targetNode, StateModel targetStateModel)
        {
            _transition.Make(targetNode, targetStateModel);
        }

        private void RemoveStateMachineNode(StateMachineNode stateMachineNode)
        {
            _runtimeStateMachineController.RemoveStateMachine(stateMachineNode.Value);
            _selectedChildStateMachines.RemoveAll(ele => ele == null);
            CreateSelectedStateMachineNodes();
        }

        private void SelectStateMachineNode(StateMachineNode stateMachineNode)
        {
            _selectedChildStateMachines.Add(stateMachineNode.Value);
            CreateSelectedStateMachineNodes();
        }

        private void GoToMachineNode(StateMachineNode stateMachineNode)
        {
            int index = _selectedChildStateMachines.IndexOf(stateMachineNode.Value);
            _selectedChildStateMachines.RemoveRange(index + 1, (_selectedChildStateMachines.Count - index) - 1);
            CreateSelectedStateMachineNodes();

        }

        private void RemoveDefaultStateModelNode(StateNode node)
        {
            _runtimeStateMachineController.RemoveDefaultState(SelectedStateMachineModel, node.Value);
            _nodes.Remove(node);

            foreach (var sNode in _nodes)
            {
                if (sNode is StateNode stateNode)
                {
                    SetAsDefaultNode(stateNode, false);
                    break;
                }
            }
        }

        private void RemoveStateModelNode(StateNode node)
        {
            _runtimeStateMachineController.RemoveState(SelectedStateMachineModel, node.Value);
            _nodes.Remove(node);
            CreateSelectedStateMachineNodes();
        }

        private void SetAsDefaultNode(StateNode stateModelNode, bool isFocused)
        {
            _runtimeStateMachineController.SetDefaultNode(stateModelNode.Value);
            stateModelNode.SetDefault(true);
            UpdateDefaultNode();
            stateModelNode.IsFocused = isFocused;
            EditorUtility.SetDirty(_runtimeStateMachineController);
        }

        private void DuplicateNode(StateNode stateModelNode)
        {
            var stateModel = stateModelNode.Value.Clone(_runtimeStateMachineController, SelectedStateMachineModel);
            CreateStateModelNode(stateModel);
        }

        private void UpdateDefaultNode()
        {
            foreach (var node in _nodes)
            {
                if (node is StateMachineNode stateMachineNode)
                    stateMachineNode.SetDefault(_runtimeStateMachineController.IsDefaultStateMachine(stateMachineNode.Value));
                else if (node is StateNode stateNode)
                    stateNode.SetDefault(stateNode.Value == _runtimeStateMachineController.GetDefaultState());
            }
        }

        private void SetCurrentActiveNode()
        {
            if (!Application.isPlaying)
                return;

            Repaint();

            if (Actor == null)
                return;

            var stateMachineModel = _runtimeStateMachineController.GetStateMachineModel(Actor?.CurrentState);
            StateMachineNode currentStateMachineNode = null;
            foreach (var node in _nodes)
            {
                if (node is StateMachineNode stateMachineNode)
                {
                    stateMachineNode.IsFocused = stateMachineNode.Value == stateMachineModel;
                    if (stateMachineNode.IsFocused)
                        currentStateMachineNode = stateMachineNode;
                }
            }

            if (stateMachineModel != SelectedStateMachineModel)
            {
                if (currentStateMachineNode != null)
                {
                    if (!stateMachineModel.IsParentOf(SelectedStateMachineModel))
                        SelectStateMachineNode(currentStateMachineNode);
                    else
                        GoToMachineNode(currentStateMachineNode);
                }
            }

            foreach (var node in _nodes)
            {
                if (node is StateNode stateNode)
                    stateNode.IsFocused = stateNode.Value?.State.Name == Actor?.CurrentState.Name;
            }

        }
    }
}
