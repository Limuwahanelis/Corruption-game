using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

//[CustomPropertyDrawer(typeof(MapObstacle))]
//public class MapObstacleDrawerUIE : PropertyDrawer
//{
//    public override VisualElement CreatePropertyGUI(SerializedProperty property)
//    {
//        VisualElement container = new VisualElement();

//        PropertyField obstacle = new PropertyField(property.FindPropertyRelative("_obstacle"));
//        PropertyField range = new PropertyField(property.FindPropertyRelative("_obstaclerange"));
//        //obstacle.RegisterCallback<GeometryChangedEvent>(AddPadding);
//        Rect aa = new Rect(0, 0, 200, 10);
//        //obstacle.style.width = 100;
//        //range.style.marginTop = 0;
//        //range.style.marginLeft = -50;
//        //range.label = "";

//        //range.style.paddingTop = 0;
//        // range.style.top = -20;
//        // range.style.left = obstacle.style.width;
//        container.Add(obstacle);
//        container.Add(range);

//        return container;
//    }

//    //private void AddPadding(GeometryChangedEvent e)
//    //{
//    //    VisualElement el=e.target as VisualElement;
//    //    el.Q<Label>().style.width = 5;
//    //    ObjectField field = el.Q<ObjectField>();
//    //    field.style.width = 50;
//    //    //field.style.marginLeft = 10;
//    //}
//}
